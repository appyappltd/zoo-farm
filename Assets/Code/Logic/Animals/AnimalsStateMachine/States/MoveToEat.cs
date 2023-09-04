using System;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsStateMachine.Transitions;
using Logic.AnimatorStateMachine;
using Logic.Movement;
using StateMachineBase.States;
using StateMachineBase.Transitions;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class MoveToEat : MoveToAndRotate
    {
        private readonly AnimalFeeder _feeder;
        private readonly Eat _eatState;
        private readonly FullBowl _fullBowlTransition;
        private readonly DistanceTo _inEatPlace;
        private Bowl _bowl;

        public MoveToEat(IPrimeAnimator animator, NavMeshMover mover, Transform target, Aligner aligner,
            AnimalFeeder feeder, Eat eatState, FullBowl fullBowlTransition, DistanceTo inEatPlace) : base(animator, mover, target, aligner)
        {
            _feeder = feeder;
            _eatState = eatState;
            _fullBowlTransition = fullBowlTransition;
            _inEatPlace = inEatPlace;
        }

        protected override void OnEnter()
        {
            FindBowl();
            base.OnEnter();
        }

        private void FindBowl()
        {
            if (_feeder.TryGetReservedBowl(out _bowl) == false)
                throw new ArgumentNullException(nameof(_bowl));
            
            Target = _bowl.EatPlace;
            _fullBowlTransition.ApplyBowl(_bowl);
            _eatState.ApplyBowl(_bowl);
            _inEatPlace.ApplyTarget(_bowl.EatPlace);
        }
    }
}