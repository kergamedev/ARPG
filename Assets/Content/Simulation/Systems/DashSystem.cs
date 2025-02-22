using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class DashSystem : SystemMainThreadFilter<DashSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform;
            public Dash* Dash;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.Transform->Position = FPVector3.MoveTowards(filter.Transform->Position, filter.Dash->Destination, filter.Dash->Speed);
            if (filter.Transform->Position == filter.Dash->Destination)
                f.Remove<Dash>(filter.Entity);
        }
    }
}
