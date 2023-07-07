using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.Animals.AnimalsStateMachine.Transitions;
using Logic.Storages;
using Logic.VolunteersStateMachine.States;
using StateMachineBase;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.VolunteersStateMachine
{
    public class VolunteerStateMachine : StateMachine
    {
        [Header("Controls")]
        [SerializeField] private VolunteerAnimator _animator;
        [SerializeField] private NavMeshMover _mover;
        
        [SerializeField, Range(.0f, 10f)] private float _placeOffset;

        private Transform transmittingPlace;
        private Transform outPlace;
        private Transform rotateTo;

        public void Construct(Transform transmittingPlace, Transform outPlace, Transform rotateTo)
        {
            this.transmittingPlace = transmittingPlace;
            this.outPlace = outPlace;
            this.rotateTo = rotateTo;

            SetUp();
        }

        private void SetUp()
        {
            Volunteers.Volunteer volunteer = GetComponent<Volunteers.Volunteer>();
            IInventory inventory = volunteer.Inventory;

            State moveToTransmitting = new MoveTo(_animator, _mover, transmittingPlace);
            State transmitting = new Transmitting(_animator, volunteer);
            State moveToOutPlace = new MoveTo(_animator, _mover, outPlace);
            State reload = new Reload(_animator, volunteer);

            Transition inTransmittingPlace = new TargetInRange(_mover.transform, transmittingPlace, _placeOffset);
            Transition emptyAnimal = new EmptyAnimal(inventory);
            Transition queueMove = new TargetOutOfRange(_mover.transform, transmittingPlace, _placeOffset);
            Transition inOutPlace = new TargetInRange(_mover.transform, outPlace, _placeOffset);
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
