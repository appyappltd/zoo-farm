using Logic.AnimalsBehaviour;
using Logic.AnimalsBehaviour.Movement;
using Logic.AnimalsStateMachine.States;
using Logic.Volunteer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveToOut : MoveTo
{
    private Volunteer volunteer;
    public MoveToOut(IPrimeAnimator animator, NavMeshMover mover, Transform target, Volunteer volunteer) : base(animator, mover, target)
    {
        this.volunteer = volunteer;
    }
    protected override void OnEnter()
    {
        base.OnEnter();
        volunteer.CanGiveAnimal = false;
    }
}
