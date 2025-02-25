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
            var lastState = filter.Character->State;
            var currentState = ResolveCharacterState(f, in filter);

            if (lastState == currentState)
                return;
            
            filter.Character->State = currentState;

            if (currentState != CharacterState.Locomotion)
            {
                filter.Character->OngoingLocomotion = LocomotionKind.NotInLocomotion;
                filter.Character->CanSprint = currentState == CharacterState.Dashing;
            }
            else
            {
                if (filter.Character->OngoingLocomotion == LocomotionKind.NotInLocomotion)
                    filter.Character->OngoingLocomotion = LocomotionKind.Idle;
            }
        }

        private CharacterState ResolveCharacterState(Frame f, in Filter filter)
        {
            var state = default(CharacterState);

            if (f.Has<AbilityAction>(filter.Entity))
                state = CharacterState.InAbility;
            else if (f.Has<DashAction>(filter.Entity))
                state = CharacterState.Dashing;
            else state = CharacterState.Locomotion;

            return state;
        }
    }
}
