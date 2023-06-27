using Logic.AnimalsBehaviour.Movement;
using Logic.AnimalsBehaviour;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic.AnimalsStateMachine.States;
using Logic.AnimalsStateMachine.Transitions;
using Logic;
using StateMachineBase;
using Logic.Inventory;
using Logic.Volunteer;

public class VolunteerStateMachine : StateMachine
{
    [Header("Controls")]
    [SerializeField] private VolunteerAnimator _animator;
    [SerializeField] private NavMeshMover _mover;

    [Header("Move Settings")]
    [Space]
    [SerializeField] private Transform _transmittingPlace;
    [SerializeField] private Transform _outPlace;
    [SerializeField, Range(.0f, 10f)] private float _placeOffset;

    public void Construct(Transform transmittingPlace, Transform outPlace)
    {
        _transmittingPlace = transmittingPlace;
        _outPlace = outPlace;

        SetUp();
    }

    private void SetUp()
    {
        var inventory = GetComponent<Inventory>();
        var volunteer = GetComponent<Volunteer>();

        State idle = new Idle(_animator);
        State moveToTransmitting = new MoveTo(_animator, _mover, _transmittingPlace);
        State moveToOutPlace = new MoveToOut(_animator, _mover, _outPlace, volunteer);
        State transmitting = new Transmitting(_animator, volunteer);

        Transition haveAnimal = new HaveAnimal(inventory);
        Transition inTransmittingPlace = new TargetInRange(_mover.transform, _transmittingPlace, _placeOffset);
        Transition inOutPlace = new TargetInRange(_mover.transform, _outPlace, _placeOffset);
        Transition emptyAnimal = new EmptyAnimal(inventory);

        Init(idle, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    idle, new Dictionary<Transition, State>
                    {
                        {haveAnimal, moveToTransmitting},
                    }
                },
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
                    }
                },
                {
                    moveToOutPlace, new Dictionary<Transition, State>
                    {
                        {inOutPlace, idle},
                    }
                },
            });
    }
}
