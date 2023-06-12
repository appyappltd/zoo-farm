using Logic.AnimalsBehaviour.Movement;
using StateMachineBase;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.AnimalsBehaviour
{
    public class JumpListener : MonoBehaviour
    {
        [SerializeField] private Jumper _jumper;
        [SerializeField] private StateMachine _stateMachine;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private AnimalMover _mover;

        private bool _isMoverEnabled;
        
        private void OnEnable()
        {
            _jumper.Jumped += OnJumped;
            _jumper.StartJump += OnStartJump;
        }

        private void OnDisable()
        {
            _jumper.Jumped -= OnJumped;
            _jumper.StartJump -= OnStartJump;
        }

        private void OnStartJump()
        {
            _stateMachine.Stop();
            _isMoverEnabled = _mover.enabled;
            _mover.enabled = false;
            _agent.enabled = false;
        }
        
        private void OnJumped()
        {
            _stateMachine.Play();
            _mover.enabled = _isMoverEnabled;
            _agent.enabled = true;
        }
    }
}