using ARPG.Config;
using ARPG.Simulation;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Physics3D;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Quantum
{
    [Serializable]
    public class DamageAF : AbilityEffect
    {
        [SerializeField]
        private FP _damage;

        [SerializeField]
        private TeamCheck _teamCheck;

        public unsafe override void Update(Frame f, ref CharacterAbilitySystem.Filter filter, AbilityConfig ability, List<Hit3D> hitsThisFrame, QHashSet<EntityHit> hitBuffer)
        {
            foreach (var hit in hitsThisFrame)
            {
                if (!f.Unsafe.TryGetPointer(hit.Entity, out Character* hitCharacter))
                    continue;

                if (!SimulationUtilities.CheckTeams(*filter.Character, *hitCharacter, _teamCheck))
                    continue;

                hitBuffer.Add(new EntityHit() { Entity = hit.Entity, Tick = f.Number });
                f.Events.EntityHit(filter.Entity, hit.Entity, _damage);
            }
        }
    }
}