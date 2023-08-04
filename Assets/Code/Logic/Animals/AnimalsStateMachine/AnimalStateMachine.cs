using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsStateMachine.States;
using Logic.Animals.AnimalsStateMachine.Transitions;
using Logic.Movement;
using NaughtyAttributes;
using Progress;
using StateMachineBase;
using StateMachineBase.States;
using StateMachineBase.Transitions;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine
{
    public class AnimalStateMachine : StateMachine
    {
        [Header("Controls")]
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private NavMeshMover _mover;

        [Header("Stats")] [Space]
        [SerializeField] private StatIndicator _vitality;
        [SerializeField] private StatIndicator _satiety;
        [SerializeField] private StatIndicator _peppiness;
        [SerializeField] private StatIndicator _age;

        [Header("Stats Settings")] [Space]
        [SerializeField] private float _satietyReplanishSpeed;
        [SerializeField] private float _peppinessReplanishSpeed;
        [SerializeField] private float _hungerDelay;

        [Header("Move Settings")] [Space]
        [SerializeField] private float _maxWanderDistance;

        [MinMaxSlider(1f, 20f)]
        [SerializeField] private Vector2 _idleDelayRange;

        [SerializeField] private float _placeOffset;

        private Transform _restPlace;
        private Transform _eatPlace;

        public void Construct(Transform houseRestPlace, Transform houseEatPlace)
        {
            _restPlace = houseRestPlace;
            _eatPlace = houseEatPlace;

            SetUp();
            EnableStatIndicators();
        }

        public void ReleaseMove(Transform to)
        {
            State moveTo = new MoveTo(_animator, _mover, to);
            ForceState(moveTo);
        }

        private void SetUp()
        {
            Bowl bowl = _eatPlace.GetComponent<Bowl>();

            State eat = new Eat(_animator, _satiety, _satietyReplanishSpeed, _hungerDelay, bowl.ProgressBarView);
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
            Transition notFullSatiety = new BarNotFullTransition(_satiety.ProgressBar);
            Transition randomDelay = new RandomTimerTransition(_idleDelayRange.y, _idleDelayRange.x);
            Transition inRestPlace = new TargetInRange(_mover.transform, _restPlace, _placeOffset);
            Transition inEatPlace = new TargetInRange(_mover.transform, _eatPlace, _placeOffset);
            Transition reachTarget = new ReachDestinationTransition(_mover);

            Init(idle, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    idle, new Dictionary<Transition, State>
                    {
                        {notFullSatiety, moveToEat},
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
                    waitForFood, new Dictionary<Transition, State>
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

        private void EnableStatIndicators()
        {
            _vitality.Enable();
            _satiety.Enable();
            _peppiness.Enable();
            _age.Enable();
        }

        private ActionTransition GetOnFullActionTransition(IProgressBarView barView)
        {
            ActionTransition transition = new ActionPreCheckTransition(() => barView.IsFull);
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