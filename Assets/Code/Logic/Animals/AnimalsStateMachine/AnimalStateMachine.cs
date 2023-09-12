using System;
using System.Collections.Generic;
using Logic.Animals.AnimalsStateMachine.Transitions;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsStateMachine.States;
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
        [SerializeField] private Aligner _aligner;
        [SerializeField] private Animal _animal;
        
        [Space] [Header("Stats")]
        [SerializeField] private StatIndicator _vitality;

        [SerializeField] private StatIndicator _satiety;
        [SerializeField] private StatIndicator _peppiness;
        [SerializeField] private StatIndicator _age;

        [Space] [Header("Stats Settings")]
        [SerializeField] private float _satietyReplanishSpeed;
        [SerializeField] private float _peppinessReplanishSpeed;
        [SerializeField] private float _hungerDelay;

        [Space][Header("Move Settings")]
        [SerializeField] private float _maxWanderDistance;
        [SerializeField] private float _breedingPositionOffset;

        [MinMaxSlider(1f, 20f)]
        [SerializeField] private Vector2 _idleDelayRange;
        [SerializeField] private float _placeOffset;

        private ForceMove _forceMove;
        private Idle _forceIdle;
        private ForceEat _forceEat;
        private FollowToBreed _followToBreeding;
        private BreedingIdle _breeding;
        private AnimalFeeder _feeder;

        public void Construct(AnimalFeeder feeder)
        {
            _feeder = feeder;
            SetUp();
            EnableStatIndicators();
        }
        
        public void ForceMove(Transform to)
        {
            _forceMove.SetNewPosition(to);
            ForceState(_forceMove);
        }

        public void SetForceBowl(Bowl bowl) =>
            _forceEat.SetBowl(bowl);

        public void ForceIdle() =>
            ForceState(_forceIdle);

        public void ForceEat()
        {
            ForceState(_forceEat);
        }

        public void InitBreeding(Transform breedPlace, Action onBreedingBegin)
        {
            _followToBreeding.Init(breedPlace, onBreedingBegin);
            ForceState(_followToBreeding);
        }

        public void BeginBreeding(Action onBreedingComplete)
        {
            _breeding.Init(onBreedingComplete);
            ForceState(_breeding);
        }

        private void SetUp()
        {
            Cleanup();

            Transition fullBowl = new FullBowl();
            Transition fullPeppiness = GetOnFullActionTransition(_peppiness.ProgressBar);
            Transition fullSatiety = GetOnFullActionTransition(_satiety.ProgressBar);
            Transition forceFullSatiety = GetOnFullActionTransition(_satiety.ProgressBar);
            Transition emptyPeppiness = GetOnEmptyActionTransition(_peppiness.ProgressBar);
            Transition emptySatiety = GetOnEmptyActionTransition(_satiety.ProgressBar);
            Transition emptySatietyAndHasFreeBowl = new EmptySatietyAndHasFreeBowl(_satiety.ProgressBar, _feeder);
            Transition randomDelay = new RandomTimerTransition(_idleDelayRange.y, _idleDelayRange.x);
            Transition inRestPlace = new TargetInRange(_mover.transform, null, _placeOffset);
            Transition inEatPlace = new TargetInRange(_mover.transform, null, _placeOffset);
            Transition reachTarget = new ReachDestinationTransition(_mover);
            Transition forceReachTarget = new ReachDestinationTransition(_mover);
            Transition reachedBreeding = new ReachDestinationTransition(_mover, _breedingPositionOffset);
            Transition waitForBreedingProcess = new TimerTransition(2f);
            
            State eat = new Eat(_animator, _satiety, _satietyReplanishSpeed, _hungerDelay, _feeder, _animal.Emotions);
            State rest = new Rest(_animator, _peppiness, _peppinessReplanishSpeed);
            State idle = new Idle(_animator);
            State waitForFood = new WaitForFood(_animator, _animal.Emotions);
            State wander = new Wander(_animator, _mover, _maxWanderDistance);
            State moveToRest = new MoveTo(_animator, _mover, null);
            State moveToEat = new MoveToEat(_animator, _mover, null, _aligner, _feeder, (Eat) eat, (FullBowl) fullBowl, (DistanceTo) inEatPlace);

            _followToBreeding = new FollowToBreed(_animator, _mover, _satiety);
            _breeding = new BreedingIdle(_animator, _satiety);

            _forceIdle = new Idle(_animator);
            _forceMove = new ForceMove(_animator, _mover, _aligner);
            _forceEat = new ForceEat(_animator, _satiety, _satietyReplanishSpeed);


            Init(idle, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    _followToBreeding, new Dictionary<Transition, State>
                    {
                        {reachedBreeding, _forceIdle}
                    }
                },
                {
                    _breeding, new Dictionary<Transition, State>
                    {
                        {waitForBreedingProcess, idle}
                    }
                },
                {
                    _forceIdle, new Dictionary<Transition, State>()
                },
                {
                    _forceMove, new Dictionary<Transition, State>
                    {
                        {forceReachTarget, _forceIdle},
                    }
                },
                {
                    _forceEat, new Dictionary<Transition, State>
                    {
                        {forceFullSatiety, _forceIdle},
                    }
                },
                {
                    idle, new Dictionary<Transition, State>
                    {
                        {emptySatietyAndHasFreeBowl, moveToEat},
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