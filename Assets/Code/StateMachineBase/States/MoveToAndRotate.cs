using Logic.Animals.AnimalsBehaviour.Movement;
using Logic.AnimatorStateMachine;
using StateMachineBase.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToAndRotate : MoveTo
{
    private Transform rotateTo;
    private NavMeshMover mover;

    public MoveToAndRotate(IPrimeAnimator animator, NavMeshMover mover, Transform target, Transform rotateTo) : base(animator, mover, target)
    {
        this.rotateTo = rotateTo;
        this.mover = mover;
    }

    protected override void OnExit()
    {
        base.OnExit();
        mover.RotateTo(rotateTo.position);
    }
}
