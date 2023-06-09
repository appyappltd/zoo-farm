using System.Collections.Generic;
using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.AnimalStats;
using Logic.AnimalsBehaviour.Movement;
using Logic.NewStateMachine.States;
using Logic.NewStateMachine.Transitions;
using NaughtyAttributes;
using Progress;
using StateMachineBase;
using UnityEngine;

namespace Logic.NewStateMachine
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

        private void Start()
        {
            State eat = new Eat(_animator, _satiety, _satietyReplanishSpeed);
            State rest = new Rest(_animator, _peppiness, _peppinessReplanishSpeed);
            State idle = new Idle(_animator);
            State wander = new Wander(_animator, _mover, _maxWanderDistance);
            State moveToRest = new MoveTo(_animator, _mover, _restPlace);
            State moveToEat = new MoveTo(_animator, _mover, _eatPlace);

            Transition fullPeppiness = SetUpOnFullActionTransition(_peppiness.ProgressBar);
            Transition fullSatiety = SetUpOnFullActionTransition(_satiety.ProgressBar);
            Transition emptyPeppiness = SetUpOnEmptyActionTransition(_peppiness.ProgressBar);
            Transition emptySatiety = SetUpOnEmptyActionTransition(_satiety.ProgressBar);
            Transition randomDelay = new RandomTimerTransition(_idleDelayRange.y, _idleDelayRange.x);
            Transition inRestPlace = new TargetInRange(_mover.transform, _restPlace, _placeOffset);
            Transition inEatPlace = new TargetInRange(_mover.transform, _eatPlace, _placeOffset);
            Transition reachTarget = new ReachDestinationTransition(_mover);

            Init(idle, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    idle, new Dictionary<Transition, State>
                    {
                        {emptyPeppiness, moveToRest},
                        {emptySatiety, moveToEat},
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
                        {inEatPlace, eat},
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

        private ActionTransition SetUpOnFullActionTransition(IProgressBarView barView)
        {
            ActionTransition transition = new ActionTransition();
            barView.Full += transition.SetConditionTrue;
            transition.SetUnsubscribeAction(() => barView.Full -= transition.SetConditionTrue);
            return transition;
        }

        private ActionTransition SetUpOnEmptyActionTransition(IProgressBarView barView)
        {
            ActionTransition transition = new ActionPreCheckTransition(() => barView.IsEmpty);
            barView.Empty += transition.SetConditionTrue;
            transition.SetUnsubscribeAction(() => barView.Empty -= transition.SetConditionTrue);
            return transition;
        }
    }
}