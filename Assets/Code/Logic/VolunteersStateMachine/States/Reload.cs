using Logic.AnimatorStateMachine;
using StateMachineBase.States;
using System.Collections;
using System.Collections.Generic;
using Logic.Volunteers;
using UnityEngine;

public class Reload : Idle
{
    private Volunteer volunteer;
    public Reload(IPrimeAnimator animator, Volunteer volunteer) : base(animator)
    {
        this.volunteer = volunteer;
    }
    protected override void OnEnter()
    {
        base.OnEnter();
    }
}
