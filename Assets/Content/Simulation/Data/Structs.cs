using System;

namespace Quantum
{
    public partial struct EntityHit : IEquatable<EntityHit>
    {
        public bool Equals(EntityHit other)
        {
            return Entity == other.Entity && Tick == other.Tick;
        }
    }
}
