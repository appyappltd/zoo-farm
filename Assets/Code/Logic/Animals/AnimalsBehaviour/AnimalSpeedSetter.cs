using System;
using Logic.Movement;
using Observables;
using StateMachineBase;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public class AnimalAgentSpeedSetter : MonoBehaviour
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private NavMeshMover _mover;

        [SerializeField] [Range(0f, 1f)] private float _normalizedWalkSpeed;
        [SerializeField] [Range(0f, 1f)] private float _normalizedRunSpeed;

        private void Awake()
        {
            _compositeDisposable.Add(_stateMachine.CurrentStateType.Then(Observer));
        }

        private void Observer(Type stateType)
        {
            if (stateType == typeof(Wander))
                _mover.SetNormalizedSpeed(_normalizedWalkSpeed);

            if (typeof(MoveTo).IsAssignableFrom(stateType))
                _mover.SetNormalizedSpeed(_normalizedRunSpeed);
        }
    }
}