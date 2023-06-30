using StateMachineBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineObserver : MonoBehaviour
{
    [SerializeField] private string _state;

    private StateMachine machine;

    private void Awake()
    {
        machine= GetComponent<StateMachine>();
        machine.CurrentStateType.Then(type => _state = type.Name);
    }
}
