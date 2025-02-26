using Quantum.Collections;
using Quantum.Physics3D;
using System;
using System.Collections.Generic;

namespace Quantum
{
    [Serializable]
    public abstract class AbilityEffect
    {
        public abstract void Update(Frame f, ref CharacterAbilitySystem.Filter filter, AbilityConfig ability, List<Hit3D> hitsThisFrame, QHashSet<EntityHit> hitBuffer);
    }
}