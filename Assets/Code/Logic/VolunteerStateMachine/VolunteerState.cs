using Infrastructure.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Code/Logic/VolunteerStateMashine/States")]
public sealed class VolunteerState : BaseState
{
    public List<FSMAction> Action = new List<FSMAction>();
    public List<VolunteerTransition> Transitions = new List<VolunteerTransition>();

    public override void Execute(VolunteerStateMachine machine)
    {
        foreach (var action in Action)
            action.Execute(machine);

        foreach (var transition in Transitions)
            transition.Execute(machine);
    }
}