using Logic.AnimalsBehaviour;
using Logic.AnimalsStateMachine.States;
using Logic.Volunteer;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Transmitting : Idle
{
    private Volunteer volunteer;

    protected override void OnEnter()
    {
        base.OnEnter();
        volunteer.CanGiveAnimal = true;
    }

    public Transmitting(IPrimeAnimator animator, Volunteer volunteer) : base(animator)
    {
        this.volunteer = volunteer;
    }
}
