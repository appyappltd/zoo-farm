using System;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;

namespace Logic.Movement
{
    public class ItemMover : MonoCache, IItemMover
    {
        [SerializeField, Min(.0f)] private float _speed = 5.0f;
        [SerializeField, Min(.0f)] private float _errorOffset = 0.1f;
        [SerializeField] private Vector3 _rotationOffset;

        private Transform _target;
        private Transform _finalParent;
        private Quaternion _finalRotation;

        private Action EndMoveCallback;
        private Action Moving;

        private bool _isModifyRotation;

        public event Action Started = () => { };
        public event Action Ended = () => { };

        private void Awake()
        {
            Moving += Translate;
            enabled = false;
        }

        public void Move(Transform to, Action OnMoveEnded, Transform finishParent = null, bool isModifyRotation = false)
        {
            EndMoveCallback = OnMoveEnded;
            Move(to, finishParent, isModifyRotation);
        }

        public void Move(Transform target, Transform finishParent = null, bool isModifyRotation = false)
        {
            if (finishParent)
                Moving += Rotate;

            _isModifyRotation = isModifyRotation;
            _target = target;
            _finalParent = finishParent;
            transform.Unparent();
            enabled = true;
            Started.Invoke();
        }

        protected override void Run()
        {
            Moving.Invoke();

            if (IsFinished())
                FinishTranslation();
        }

        private void Rotate()
        {
            float distanceToTarget = GetDistanceToTarget();
            transform.rotation = Quaternion.Lerp(transform.rotation, GetFinalRotation(),
                Time.deltaTime * _speed / distanceToTarget);
        }

        private void Translate()
        {
            Vector3 translateDirection = (_target.position - transform.position).normalized;
            Vector3 deltaTranslation = translateDirection * _speed * Time.deltaTime;
            transform.Translate(deltaTranslation, Space.World);
        }

        private void FinishTranslation()
        {
            enabled = false;
            transform.SetParent(_finalParent, true);

            if (_finalParent)
            {
                Moving -= Rotate;
                transform.rotation = GetFinalRotation();
            }

            Ended.Invoke();
        }

        private bool IsFinished() =>
            GetDistanceToTarget() < _errorOffset;

        private Quaternion GetFinalRotation()
        {
            return _isModifyRotation
                ? _finalParent.rotation * Quaternion.Euler(_rotationOffset)
                : _finalParent.rotation;
        }

        private float GetDistanceToTarget() =>
            Vector3.Distance(transform.position, _target.position);
    }
}