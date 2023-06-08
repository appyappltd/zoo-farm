using System;
using System.Collections.Generic;
using Logic.AnimalsBehaviour.States;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class AnimalStateMachine : MonoStateMachine.MonoStateMachine
    {
        protected override void InitTransitions()
        {
            foreach (ITransition transition in GetComponentsInChildren<ITransition>(true))
            {
                transition.Init(this);
                transition.Disable();
            }
        }

        protected override void SetDefaultState()
        {
            Enter<AnimalIdleState>();
        }

        protected override Dictionary<Type, IMonoState> GetStates()
        {
            foreach (var component in GetComponentsInChildren<Component>())
            {
                Debug.Log(component.GetType());
            }

            return new Dictionary<Type, IMonoState>
            {
                [typeof(AnimalIdleState)] = GetComponentInChildren<AnimalIdleState>(true),
                [typeof(AnimalMoveState)] = GetComponentInChildren<AnimalMoveState>(true),
                [typeof(AnimalRestState)] = GetComponentInChildren<AnimalRestState>(true),
            };
        }
    }
}