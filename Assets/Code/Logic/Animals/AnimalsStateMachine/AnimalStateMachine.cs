using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.Animals.AnimalsStateMachine.States;
using Logic.Animals.AnimalsStateMachine.Transitions;
using NaughtyAttributes;
using Progress;
using StateMachineBase;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine
{
    public class AnimalStateMachine : StateMachine
    {
        [Header("Controls")]
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private AnimalMover _mover;

        [Header("Stats")] [Space]
        [SerializeField] private ProgressBarIndicator _vitality;
        [SerializeField] private ProgressBarIndicator _satiety;
        [SerializeField] private ProgressBarIndicator _peppiness;

        [Header("Stats Changing Speed")] [Space]
        [SerializeField] private float _satietyReplanishSpeed;
        [SerializeField] private float _peppinessReplanishSpeed;

        [Header("Move Settings")] [Space]
        [SerializeField] private float _maxWanderDistance;
        [SerializeField] private Transform _restPlace;
        [SerializeField] private Transform _eatPlace;

        [MinMaxSlider(1f, 20f)]
        [SerializeField] private Vector2 _idleDelayRange;
        [SerializeField] private float _placeOffset;

        public void Construct(Transform houseRestPlace, Transform houseEatPlace)
        {
            _restPlace = houseRestPlace;
            _eatPlace = houseEatPlace;

            SetUp();
        }

        private void SetUp()
        {
            Bowl bowl = _eatPlace.GetComponent<Bowl>();

            State eat = new Eat(_animator, _satiety, _satietyReplanishSpeed, bowl.ProgressBarView);
            State rest = new Rest(_animator, _peppiness, _peppinessReplanishSpeed);
            State idle = new Idle(_animator);
            State waitForFood = new Idle(_animator);
            State wander = new Wander(_animator, _mover, _maxWanderDistance);
            State moveToRest = new MoveTo(_animator, _mover, _restPlace);
            State moveToEat = new MoveTo(_animator, _mover, _eatPlace);

            Transition fullBowl = GetOnFullActionTransition(bowl.ProgressBarView);
            Transition fullPeppiness = GetOnFullActionTransition(_peppiness.ProgressBar);
            Transition fullSatiety = GetOnFullActionTransition(_satiety.ProgressBar);
            Transition emptyPeppiness = GetOnEmptyActionTransition(_peppiness.ProgressBar);
            Transition emptySatiety = GetOnEmptyActionTransition(_satiety.ProgressBar);
            Transition randomDelay = new RandomTimerTransition(_idleDelayRange.y, _idleDelayRange.x);
            Transition inRestPlace = new TargetInRange(_mover.transform, _restPlace, _placeOffset);
            Transition inEatPlace = new TargetInRange(_mover.transform, _eatPlace, _placeOffset);
            Transition reachTarget = new ReachDestinationTransition(_mover);

            Init(idle, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    idle, new Dictionary<Transition, State>
                    {
                        {emptySatiety, moveToEat},
                        {emptyPeppiness, moveToRest},
                        {randomDelay, wander},
                    }
                },
                {
                    wander, new Dictionary<Transition, State>
                    {
                        {reachTarget, idle},
                    }
                },
                {
                    moveToEat, new Dictionary<Transition, State>
                    {
                        {inEatPlace, waitForFood},
                    }
                },
                {
                    waitForFood , new Dictionary<Transition, State>
                    {
                        {fullBowl, eat}
                    }
                },
                {
                    moveToRest, new Dictionary<Transition, State>
                    {
                        {inRestPlace, rest},
                    }
                },
                {
                    eat, new Dictionary<Transition, State>
                    {
                        {fullSatiety, idle},
                    }
                },
                {
                    rest, new Dictionary<Transition, State>
                    {
                        {fullPeppiness, idle},
                    }
                },
            });
        }

        private ActionTransition GetOnFullActionTransition(IProgressBarView barView)
        {
            ActionTransition transition = new ActionTransition();
            barView.Full += transition.SetConditionTrue;
            transition.SetUnsubscribeAction(() => barView.Full -= transition.SetConditionTrue);
            return transition;
        }

        private ActionTransition GetOnEmptyActionTransition(IProgressBarView barView)
        {
            ActionTransition transition = new ActionPreCheckTransition(() => barView.IsEmpty);
            barView.Empty += transition.SetConditionTrue;
            transition.SetUnsubscribeAction(() => barView.Empty -= transition.SetConditionTrue);
            return transition;
        }
    }
}