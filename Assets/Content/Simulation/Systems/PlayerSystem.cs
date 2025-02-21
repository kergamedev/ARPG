using Photon.Deterministic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class PlayerSystem : SystemMainThreadFilter<PlayerSystem.Filter>, ISignalOnPlayerAdded
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public PlayerLink* Player;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = default(Input*);
            if (f.Unsafe.TryGetPointer(filter.Entity, out PlayerLink* playerLink))
                input = f.GetPlayerInput(playerLink->Player);

            if (input->Move.X != 0 || input->Move.Y != 0)
            {
                var playerConfig = f.RuntimeConfig.GetPlayerConfig(f);

                var moveSpeed = playerConfig.MoveSpeed;
                var start = filter.Transform->Position;
                var startHit = f.Physics3D.Raycast(start + FPVector3.Up * 50, FPVector3.Down, 100, f.RuntimeConfig.GetGameConfig(f).FloorMask, QueryOptions.HitStatics | QueryOptions.ComputeDetailedInfo);
                if (startHit != null)
                    moveSpeed *= FPMath.Clamp01(FPVector3.Dot(FPVector3.Up, startHit.Value.Normal));

                var end = start + (input->Move * moveSpeed * f.DeltaTime).XOY;
                
                var navmesh = f.Map.NavMeshes.First().Value;
                end = navmesh.MovePositionIntoNavmesh(f, start.XZ, end.XZ, 1, NavMeshRegionMask.Default).XOY;

                var endHit = f.Physics3D.Raycast(end + FPVector3.Up * 50, FPVector3.Down, 100, f.RuntimeConfig.GetGameConfig(f).FloorMask, QueryOptions.HitStatics);
                if (endHit != null)
                    end.Y = endHit.Value.Point.Y;

                filter.Transform->Position = end;
                filter.Transform->Rotation = FPQuaternion.FromToRotation(FPVector3.Forward, input->Move.XOY.Normalized);
            }
        }

        void ISignalOnPlayerAdded.OnPlayerAdded(Frame f, PlayerRef player, bool firstTime)
        {
            var playerData = f.GetPlayerData(player);
            var playerPrototype = f.FindAsset(playerData.PlayerAvatar);
            var playerEntity = f.Create(playerPrototype);
            f.Add(playerEntity, new PlayerLink() { Player = player });

            f.Events.PlayerSpawned(player, playerEntity);
        }
    }
}
