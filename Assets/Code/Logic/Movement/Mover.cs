using NTC.Global.Cache;
using Tools.Extension;
using UnityEngine;

namespace Logic.Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Mover : MonoCache
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private Rigidbody _rigidbody;

        private Vector3 _currentVelocity;
        private Vector3 _moveDirection;

        protected float Speed => _speed;
        protected Rigidbody Rigidbody => _rigidbody;

        private void Awake() =>
            _rigidbody ??= GetComponent<Rigidbody>();

        protected override void FixedRun()
        {
            Move();
            Rotate();
        }

        protected abstract Vector3 GetMoveDirection();
        protected abstract Quaternion GetLookRotation();

        private void Move()
        {
            Vector3 moveDirection = GetMoveDirection();
            Vector3 velocity = moveDirection * _speed;
            _rigidbody.velocity = velocity.ChangeY(_rigidbody.velocity.y);
        }

        private void Rotate()
        {
            Quaternion lookRotation = GetLookRotation();
            Quaternion targetRotation = Quaternion.Slerp(_rigidbody.rotation, lookRotation, _rotateSpeed * Time.fixedDeltaTime);

            _rigidbody.MoveRotation(targetRotation);
        }
    }
}