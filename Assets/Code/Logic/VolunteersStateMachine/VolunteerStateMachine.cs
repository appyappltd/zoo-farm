using System.Collections.Generic;
using Logic.Movement;
using Logic.Storages;
using Logic.Volunteers;
using Logic.Volunteers.Queue;
using Logic.VolunteersStateMachine.States;
using StateMachineBase;
using StateMachineBase.States;
using StateMachineBase.Transitions;
using UnityEngine;

namespace Logic.VolunteersStateMachine
{
    public class VolunteerStateMachine : StateMachine
    {
        [Header("References")]
        [SerializeField] private VolunteerAnimator _animator;
        [SerializeField] private NavMeshMover _mover;

        [Space] [Header("Settings")]
        [SerializeField, Range(.0f, 1f)] private float _placeOffset;

        private Volunteer _volunteer;
        private Transform _outPlace;
        private QueueOperator _queueOperator;

        public void Construct(QueueOperator queueOperator, Transform outPlace, Volunteer volunteer)
        {
            _queueOperator = queueOperator;
            _outPlace = outPlace;
            _volunteer = volunteer;

            SetUp();
        }

        private void SetUp()
        {
            IInventory inventory = _volunteer.Inventory;
            Transform tailTransform = _queueOperator.Tail.transform;
            Transform moverTransform = _mover.transform;

            State waiting = new Idle(_animator);
            State moveInQueue = new MoveToQueuePlace(_animator, _mover, null, _volunteer);
            State moveToQueueEnd = new MoveToPosition(_animator, _mover, tailTransform);
            State transmitting = new Transmitting(_animator, _volunteer);
            State moveToOutPlace = new MoveToPosition(_animator, _mover, _outPlace);
            State reload = new Reload(_animator, _volunteer);

            Transition inTransmittingPlace = new PositionInRange(moverTransform, _queueOperator.Head.transform, _placeOffset);
            Transition inQueueEndPlace = new PositionInRange(moverTransform, tailTransform, _placeOffset);
            Transition inQueuePlace = new InQueuePlace(moverTransform, null , _volunteer, _placeOffset);
            Transition emptyAnimal = new EmptyAnimal(inventory);
            Transition queueMoved = new OutOfQueuePlace(moverTransform, null, _volunteer, _placeOffset);
            Transition inOutPlace = new TargetInRange(moverTransform, _outPlace, _placeOffset);
            Transition haveAnimal = new HaveAnimal(inventory);

            Init(moveToQueueEnd, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    moveToQueueEnd, new Dictionary<Transition, State>
                    {
                        {inQueueEndPlace, moveInQueue},
                    }
                },
                {
                    moveInQueue, new Dictionary<Transition, State>
                    {
                        {inQueuePlace, waiting},
                    }
                },
                {
                    waiting, new Dictionary<Transition, State>
                    {
                        {queueMoved, moveInQueue},
                        {inTransmittingPlace, transmitting},
                    }
                },
                {
                    transmitting, new Dictionary<Transition, State>
                    {
                        {emptyAnimal, moveToOutPlace},
                    }
                },
                {
                    moveToOutPlace, new Dictionary<Transition, State>
                    {
                        {inOutPlace, reload},
                    }
                },
                {
                    reload, new Dictionary<Transition, State>
                    {
                        {haveAnimal, moveToQueueEnd},
                    }
                },
            });
        }
    }
}
