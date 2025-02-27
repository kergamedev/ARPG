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

        public static void Perform(Frame f, EntityRef entity, FPVector3 destination, FP speed)
        {
            if (!f.Unsafe.TryGetPointer(entity, out Character* character) ||
                !f.Unsafe.TryGetPointer(entity, out Transform3D* transform))
                return;

            f.Add(entity,
                new DashAction()
                {
                    Destination = destination,
                    Speed = speed
                });

            f.Events.CharacterDashed(entity, transform->Position, destination);
        }

        public override void Update(Frame f, ref Filter filter)
        {
            filter.Transform->Position = FPVector3.MoveTowards(filter.Transform->Position, filter.Action->Destination, filter.Action->Speed * f.DeltaTime);
            if (filter.Transform->Position == filter.Action->Destination)
                f.Remove<DashAction>(filter.Entity);
        }
    }
}
