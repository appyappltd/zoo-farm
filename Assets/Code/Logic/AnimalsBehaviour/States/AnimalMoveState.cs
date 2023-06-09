using Logic.AnimalsBehaviour.Movement;
using MonoStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour.States
{
    public class AnimalMoveState : PayloadedState<Vector3>
    {
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private AnimalMover _mover;
        [SerializeField] private float _maxMoveDistance;

        private Vector3 _payloadPosition;
        private bool _isHasPayload;
        
        protected override void Run() =>
            _animator.SetSpeed(_mover.NormalizedSpeed);

        protected override void OnEnter()
        {
            _animator.SetMove();

            if (_isHasPayload)
            {
                BeginMove(_payloadPosition);
                return;
            }
            
            Vector3 randomOffset = new Vector3(
                Random.Range(-_maxMoveDistance, _maxMoveDistance),
                0,
                Random.Range(-_maxMoveDistance, _maxMoveDistance));

            Vector3 destination = transform.position + randomOffset;
            BeginMove(destination);
        }

        private void BeginMove(Vector3 to)
        {
            _mover.SetDestination(to);
            _mover.enabled = true;
        }

        protected override void OnDisabled()
        {
            _mover.enabled = false;
            _isHasPayload = false;
        }

        protected override void OnPayloadEnter(Vector3 payload)
        {
            _payloadPosition = payload;
            _isHasPayload = true;
        }
    }
}