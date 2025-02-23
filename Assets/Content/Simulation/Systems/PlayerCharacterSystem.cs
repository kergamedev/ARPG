using Photon.Deterministic;
using Quantum.Physics3D;
using System.Linq;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class PlayerCharacterSystem : SystemMainThreadFilter<PlayerCharacterSystem.Filter>, ISignalOnPlayerAdded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public PlayerCharacter* Player;
            public Character* Character;
            public Transform3D* Transform;
        }

        private readonly struct ExecutionContext
        {
            public readonly GameConfig GameConfig;
            public readonly PlayerConfig PlayerConfig;
            public readonly NavMesh NavMesh;
            public readonly Hit3D? CurrentPositionInfo;

            public ExecutionContext(Frame f, in Filter filter)
            {
                GameConfig = f.RuntimeConfig.GetGameConfig(f);
                PlayerConfig = f.RuntimeConfig.GetPlayerConfig(f);
                NavMesh = f.Map.NavMeshes.First().Value;
                CurrentPositionInfo = f.Physics3D.Raycast(filter.Transform->Position + FPVector3.Up * 50, FPVector3.Down, 100, f.RuntimeConfig.GetGameConfig(f).FloorMask, QueryOptions.HitStatics | QueryOptions.ComputeDetailedInfo);
            }
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.Player->Owner);
            var context = new ExecutionContext(f, in filter);

            if (filter.Character->State == CharacterState.Locomotion)
                UpdateMovement(f, ref filter, input, context);

            if (filter.Character->State != CharacterState.Dashing && input->Dash.WasPressed)
                TryPerformDash(f, ref filter, input, context);
        }

        private void UpdateMovement(Frame f, ref Filter filter, Input* input, in ExecutionContext context)
        {
            var start = filter.Transform->Position;

            if (input->Move.X == 0 && input->Move.Y == 0)
            {
                filter.Character->OngoingLocomotion = LocomotionKind.Idle;
                filter.Character->CanSprint = false;
                return;
            }

            var moveSpeed = context.PlayerConfig.MoveSpeed;
            var locomotionKind = LocomotionKind.Run;

            if (filter.Character->CanSprint)
            {
                moveSpeed *= context.PlayerConfig.SprintSpeedFactor;
                locomotionKind = LocomotionKind.Sprint;
            }
            else if (input->Move.Magnitude < context.PlayerConfig.RunThreshold)
            {
                moveSpeed *= context.PlayerConfig.WalkSpeedFactor;
                locomotionKind = LocomotionKind.Walk;
            }

            if (context.CurrentPositionInfo != null)
                moveSpeed *= FPMath.Clamp01(FPVector3.Dot(FPVector3.Up, context.CurrentPositionInfo.Value.Normal));
      
            var end = start + (input->Move.Normalized * moveSpeed * f.DeltaTime).XOY;       
            end = Navmesh2DToWorld(f, context.NavMesh.MovePositionIntoNavmesh(f, start.XZ, end.XZ, 1, NavMeshRegionMask.Default));

            filter.Transform->Position = end;
            filter.Transform->Rotation = FPQuaternion.FromToRotation(FPVector3.Forward, input->Move.XOY.Normalized);
            filter.Character->OngoingLocomotion = locomotionKind;
        }

        private void TryPerformDash(Frame f, ref Filter filter, Input* input, in ExecutionContext context)
        {
            var up = FPVector3.Up;
            if (context.CurrentPositionInfo != null)
                up = context.CurrentPositionInfo.Value.Normal;

            var direction = FPVector3.ProjectOnPlane(filter.Transform->Forward, up).Normalized;
            var end = filter.Transform->Position + direction * context.PlayerConfig.DashDistance;

            var canDash = false;
            var correction = context.NavMesh.MovePositionIntoNavmesh(f, filter.Transform->Position.XZ, end.XZ, 1, NavMeshRegionMask.Default);

            if (correction != end.XZ)
            {
                var remainingLookAhead = context.PlayerConfig.MaxDashLookAhead;
                while (remainingLookAhead > 0)
                {
                    end += direction * context.PlayerConfig.DashLookAheadIncrement;
                    remainingLookAhead -= context.PlayerConfig.DashLookAheadIncrement;

                    var groundCheck = f.Physics3D.Raycast(end + FPVector3.Up * 50, FPVector3.Down, 100, f.RuntimeConfig.GetGameConfig(f).FloorMask, QueryOptions.HitStatics);
                    if (groundCheck != null)
                    {
                        if (context.NavMesh.Contains(end, NavMeshRegionMask.Default))
                        {
                            canDash = true;
                            break;
                        }
                        else if (context.NavMesh.FindClosestTriangle(f, end, context.NavMesh.MinAgentRadius, NavMeshRegionMask.Default, out _, out var closestPoint))
                        {
                            end = closestPoint;
                            canDash = true;
                            break;
                        }
                    }
                }
            }
            else canDash = true;

            if (!canDash)
                return;

            var elapsedTimeSinceLastDash = (f.Number - filter.Player->LastDashTick) * f.DeltaTime;
            var isLookAheadDash = !f.Navigation.LineOfSight(filter.Transform->Position.XZ, end.XZ, NavMeshRegionMask.Default, context.NavMesh);

            if (elapsedTimeSinceLastDash < context.PlayerConfig.DashCooldown && !isLookAheadDash)
                return;

            CharacterDashSystem.Perform(f, filter.Entity, Navmesh2DToWorld(f, end.XZ), context.PlayerConfig.DashSpeed);
            filter.Player->LastDashTick = f.Number;
        }

        #region Callbacks

        void ISignalOnPlayerAdded.OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var playerData = f.GetPlayerData(player);
            var playerPrototype = f.FindAsset(playerData.PlayerAvatar);
            var playerEntity = f.Create(playerPrototype);
            f.Add(playerEntity, new PlayerCharacter() { Owner = player });

            f.Events.PlayerCharacterSpawned(player, playerEntity);
        }

        #endregion

        #region Utility

        private FPVector3 Navmesh2DToWorld(Frame f, FPVector2 position2D)
        {
            var position3D = position2D.XOY;

            var endHit = f.Physics3D.Raycast(position3D + FPVector3.Up * 50, FPVector3.Down, 100, f.RuntimeConfig.GetGameConfig(f).FloorMask, QueryOptions.HitStatics);
            if (endHit != null)
                position3D.Y = endHit.Value.Point.Y;

            return position3D;
        }

        #endregion
    }
}
