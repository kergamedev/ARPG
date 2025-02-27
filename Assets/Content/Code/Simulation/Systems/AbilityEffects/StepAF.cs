using ARPG.Config;
using ARPG.Simulation;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Physics3D;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public class StepAF : AbilityEffect
    {
        [SerializeField]
        private FPAnimationCurve _curve;

        [SerializeField]
        private FP _minDistanceFromClosestTarget;

        public unsafe override void Update(Frame f, ref CharacterAbilitySystem.Filter filter, AbilityConfig ability, List<Hit3D> hitsThisFrame, QHashSet<EntityHit> hitBuffer)
        {
            if (!ability.TryGetElapsedTimeSinceEventStart(AbilityEvent.Step, filter.Action->Progress, out var elpasedTime))
                return;

            var gameConfig = f.RuntimeConfig.GetGameConfig(f);
            if (SimulationUtilities.TryGetClosestCharacter(f, filter.Entity, gameConfig.ClosestCharacterMaxSearchDistance, TeamCheck.OpposingTeams, out var closestTarget, out _))
            {
                var closestTargetTransform = f.Unsafe.GetPointer<Transform3D>(closestTarget);
                var distance = FPVector3.Distance(filter.Transform->Position, closestTargetTransform->Position);
                if (distance < _minDistanceFromClosestTarget)
                    return;
            }

            var navMesh = f.Map.NavMeshes.First().Value;

            var offsetDistance = _curve.Evaluate(elpasedTime);
            var offset = (filter.Action->StartRotation * FPVector3.Forward) * offsetDistance;
            var start = filter.Transform->Position;
            var end = filter.Action->StartPosition + offset;

            filter.Transform->Position = SimulationUtilities.Navmesh2DToWorld(f, navMesh.MovePositionIntoNavmesh(f, start.XZ, end.XZ, 1, NavMeshRegionMask.Default));
        }
    }
}