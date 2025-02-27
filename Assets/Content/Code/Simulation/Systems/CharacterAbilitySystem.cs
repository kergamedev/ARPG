using ARPG.Config;
using ARPG.Simulation;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Physics3D;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class CharacterAbilitySystem : SystemMainThreadFilter<CharacterAbilitySystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Character* Character;
            public Transform3D* Transform;
            public AbilityAction* Action;
        }

        public static void Perform(Frame f, EntityRef entity, AssetRef<AbilityConfig> ability)
        {
            if (!f.Unsafe.TryGetPointer(entity, out Character* character) ||
                !f.Unsafe.TryGetPointer(entity, out Transform3D* transform))
                return;

            var gameConfig = f.RuntimeConfig.GetGameConfig(f);
            if (SimulationUtilities.TryGetClosestCharacter(f, entity, gameConfig.ClosestCharacterMaxSearchDistance, TeamCheck.OpposingTeams, out var closestTarget, out _))
            {
                var closestTargetTransform = f.Unsafe.GetPointer<Transform3D>(closestTarget);
                var direction = FPVector3.ProjectOnPlane(closestTargetTransform->Position - transform->Position, FPVector3.Up).Normalized;
                transform->Rotation = FPQuaternion.FromToRotation(FPVector3.Forward, direction);
            }

            f.Add(entity,
                new AbilityAction()
                {
                    Ability = ability,
                    StartTick = f.Number,
                    StartPosition = transform->Position,
                    StartRotation = transform->Rotation
                });
        }

        public static void Interrupt(Frame f, EntityRef entity)
        {
            if (!f.Unsafe.TryGetPointer(entity, out AbilityAction* ongoingAbility) ||
                !f.Unsafe.TryGetPointer(entity, out Character* character) ||
                !ongoingAbility->CanBeInterrupted)
                return;

            f.Remove<AbilityAction>(entity);
            character->State = CharacterState.Locomotion;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var ability = f.FindAsset(filter.Action->Ability);
            var hitBuffer = f.ResolveHashSet(filter.Action->AlreadyHitEntities);

            filter.Action->Progress += f.DeltaTime * ability.SpeedFactor;
            if (!filter.Action->CanBeInterrupted && ability.HasEventBeenTriggered(AbilityEvent.Recovery, filter.Action->Progress))
                filter.Action->CanBeInterrupted = true;

            UpdateHitBuffer(f, ability, hitBuffer);
            var hitsThisFrame = GetHits(f, in filter, ability, hitBuffer);
            UpdateEffects(f, ref filter, ability, hitsThisFrame, hitBuffer);

            if (filter.Action->Progress >= ability.Duration)
                f.Remove<AbilityAction>(filter.Entity);
        }

        private void UpdateHitBuffer(Frame f, AbilityConfig ability, QHashSet<EntityHit> hitBuffer)
        {          
            var hitsToRemove = new List<EntityHit>();
            foreach (var hit in hitBuffer)
            {
                var elpasedTimeSinceHit = (f.Number - hit.Tick) * f.DeltaTime;
                if (elpasedTimeSinceHit > ability.HitBuffer)
                    hitsToRemove.Add(hit);
            }

            foreach (var hitToRemove in hitsToRemove)
                hitBuffer.Remove(hitToRemove);
        }

        private List<Hit3D> GetHits(Frame f, in Filter filter, AbilityConfig ability, QHashSet<EntityHit> alreadyHitEntities)
        {
            var gameConfig = f.RuntimeConfig.GetGameConfig(f);
            var allHits = new List<Hit3D>();

            foreach (var animatedHitBox in ability.AnimatedHitBoxes)
            {
                if (animatedHitBox.TryEvaluateShape(filter.Action->Progress, out var transform, out var shape))
                {
                    transform.Position = filter.Transform->TransformPoint(transform.Position);
                    transform.Rotation = filter.Transform->Rotation * transform.Rotation;

                    var hits = f.Physics3D.OverlapShape(transform, shape, gameConfig.CharacterMask);
                    for (var i = 0; i < hits.Count; i++)
                    {
                        var hit = hits[i];
                        if (hit.Entity == filter.Entity)
                            continue;

                        var hasAlreadyBeenHit = false;
                        foreach (var exisitingHit in alreadyHitEntities)
                        {
                            if (exisitingHit.Entity != hit.Entity)
                                continue;

                            hasAlreadyBeenHit = true;
                            break;
                        }

                        if (hasAlreadyBeenHit)
                            continue;

                        allHits.Add(hit);
                    }
                }
            }

            return allHits;
        }

        private void UpdateEffects(Frame f, ref Filter filter, AbilityConfig ability, List<Hit3D> hitsThisFrame, QHashSet<EntityHit> hitBuffer)
        {
            foreach (var effect in ability.Effects)
                effect.Update(f, ref filter, ability, hitsThisFrame, hitBuffer);
        }
    }
}
