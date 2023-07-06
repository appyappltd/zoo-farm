using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Movement
{
    public class ItemMover : MonoCache, IItemMover
    {
        [SerializeField, Min(.0f)] private float _speed = 5.0f;
        [SerializeField, Min(.0f)] private float _errorOffset = 0.1f;

        private Transform _target;
        private Transform _finalParent;

        private Action EndMoveCallback;

        public event Action Started = () => { };
        public event Action Ended = () => { };

        private void Awake() =>
            enabled = false;

        public void Move(Transform to, Action OnMoveEnded, Transform finishParent = null)
        {
            EndMoveCallback = OnMoveEnded;
            Move(to, finishParent);
        }

        public void Move(Transform target, Transform finishParent = null)
        {
            _target = target;
            _finalParent = finishParent;
            transform.SetParent(null);
            enabled = true;
            Started.Invoke();
        }

        protected override void Run()
        {
            Translate();

            if (IsFinished())
                FinishTranslation();
        }

        private void FinishTranslation()
        {
            enabled = false;
            transform.SetParent(_finalParent, true);
            Ended.Invoke();
        }

        private bool IsFinished() =>
            Vector3.Distance(transform.position, _target.position) < _errorOffset;

        private void Translate()
        {
            Vector3 translateDirection = (_target.position - transform.position).normalized;
            Vector3 deltaTranslation = translateDirection * _speed * Time.deltaTime;
            transform.Translate(deltaTranslation, Space.World);
        }
    }
}
