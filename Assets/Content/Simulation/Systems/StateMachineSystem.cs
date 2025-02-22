using UnityEngine.Scripting;

namespace Quantum
{
    [Preserve]
    public unsafe class StateMachineSystem : SystemMainThreadFilter<StateMachineSystem.Filter>
    {
        public struct Filter
        {
            public EntityRef Entity;
            public StateMachine* StateMachine;
        }

        public override void Update(Frame f, ref Filter filter)
        {
            if (f.Unsafe.TryGetPointer(filter.Entity, out Dash* _))
                filter.StateMachine->State = State.Dashing;
            else filter.StateMachine->State = State.Locomotion;
        }
    }
}
