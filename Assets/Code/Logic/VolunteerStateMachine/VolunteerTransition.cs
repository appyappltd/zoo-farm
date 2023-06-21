using UnityEngine;

[CreateAssetMenu(menuName = "FSM/Transition")]
public sealed class VolunteerTransition : ScriptableObject
{
    public Decision Decision;
    public BaseState TrueState;
    public BaseState FalseState;

    public void Execute(VolunteerStateMachine stateMachine)
    {
        if (Decision.Decide(stateMachine) && !(TrueState is RemainInState))
            stateMachine.CurrentState = TrueState;
        else if (!(FalseState is RemainInState))
            stateMachine.CurrentState = FalseState;
    }
}