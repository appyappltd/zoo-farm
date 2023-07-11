using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.Storages;
using Logic.Volunteers;
using Logic.VolunteersStateMachine.States;
using StateMachineBase;
using StateMachineBase.States;
using StateMachineBase.Transitions;
using UnityEngine;

namespace Logic.VolunteersStateMachine
{
    public class VolunteerStateMachine : StateMachine
    {
        [Header("Controls")]
        [SerializeField] private VolunteerAnimator _animator;
        [SerializeField] private NavMeshMover _mover;

        [SerializeField, Range(.0f, 1f)] private float _placeOffset;

        private Volunteer _volunteer;
        private Transform _transmittingPlace;
        private Transform _outPlace;
        private Transform rotateTo;

        public void Construct(Transform transmittingPlace, Transform outPlace, Transform rotateTo, Volunteer volunteer)
        {
            _transmittingPlace = transmittingPlace;
            _outPlace = outPlace;
            this.rotateTo = rotateTo;
            _volunteer = volunteer;

            SetUp();
        }

        private void SetUp()
        {
            IInventory inventory = _volunteer.Inventory;

            State moveToTransmitting = new MoveTo(_animator, _mover, _transmittingPlace);
            State transmitting = new Transmitting(_animator, _volunteer);
            State moveToOutPlace = new MoveTo(_animator, _mover, _outPlace);
            State reload = new Reload(_animator, _volunteer);

            Transition inTransmittingPlace = new PositionInRange(_mover.transform, _transmittingPlace, _placeOffset);
            Transition emptyAnimal = new EmptyAnimal(inventory);
            Transition queueMove = new TargetOutOfRange(_mover.transform, _transmittingPlace, _placeOffset);
            Transition inOutPlace = new PositionInRange(_mover.transform, _outPlace, _placeOffset);
            Transition haveAnimal = new HaveAnimal(inventory);

            Init(moveToTransmitting, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    moveToTransmitting, new Dictionary<Transition, State>
                    {
                        {inTransmittingPlace, transmitting},
                    }
                },
                {
                    transmitting, new Dictionary<Transition, State>
                    {
                        { emptyAnimal, moveToOutPlace},
                        { queueMove, moveToTransmitting},
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
                        {haveAnimal, moveToTransmitting},
                    }
                },
            });
        }
    }
}
