using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class CharacterDashSystem : SystemMainThreadFilter<CharacterDashSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public Character* Character;
            public DashAction* Action;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.Transform->Position = FPVector3.MoveTowards(filter.Transform->Position, filter.Action->Destination, filter.Action->Speed * f.DeltaTime);
            if (filter.Transform->Position == filter.Action->Destination)
                f.Remove<DashAction>(filter.Entity);
        }
    }
}
