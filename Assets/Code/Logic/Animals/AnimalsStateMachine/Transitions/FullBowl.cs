using System;
using StateMachineBase;

namespace Logic.Animals.AnimalsStateMachine.Transitions
{
    public class FullBowl : Transition
    {
        private Bowl _bowl;

        public override bool CheckCondition() =>
            _bowl.ProgressBarView.IsFull;

        public override void Exit() =>
            _bowl = null;

        public void ApplyBowl(Bowl bowl) =>
            _bowl = bowl ? bowl : throw new NullReferenceException(nameof(bowl));
    }
}