using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class CharacterStateSystem : SystemMainThreadFilter<CharacterStateSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public Character* Character;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (f.Unsafe.TryGetPointer(filter.Entity, out DashAction* _))
                filter.Character->State = CharacterState.Dashing;
            else filter.Character->State = CharacterState.Locomotion;

            if (filter.Character->State != CharacterState.Locomotion)
                filter.Character->OngoingLocomotion = LocomotionKind.NotInLocomotion;
        }
    }
}
