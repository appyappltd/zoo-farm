using Logic.AnimatorStateMachine;
using Logic.Volunteer;
using StateMachineBase.States;
using System.Collections;
using System.Collections.Generic;
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
