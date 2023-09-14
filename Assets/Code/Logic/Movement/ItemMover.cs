using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;

namespace Logic.Movement
{
    public class ItemMover : MonoCache, IItemMover
    {
        [SerializeField] [Min(.0f)] private float _speed = 5.0f;
        [SerializeField] [Min(.0f)] private float _errorOffset = 0.1f;
        [SerializeField] [MinValue(0f)] [MaxValue(1f)] private float _power = 0.5f;
        [SerializeField] private Vector3 _rotationOffset;
        [SerializeField] private bool _isCanChangeScale;
        [SerializeField] [ShowIf(nameof(_isCanChangeScale))] private AnimationCurve _scaleCurve;

        private Transform _target;
        private Transform _finalParent;
        private Quaternion _finalRotation;

        private Action _endMoveCallback = () => { };
        private Action<float> _moving;

        private bool _isModifyRotation;
        private DistanceBasedScaleModifier _scaleModifier;

        public event Action Started = () => { };
        public event Action Ended = () => { };

        public Transform Target => _target;

        private void Awake()
        {
            _scaleModifier = new DistanceBasedScaleModifier();
            _moving = LerpTranslate;
            enabled = false;
        }

        public void Move(Transform to, Action onMoveEnded, Transform finishParent = null, bool isModifyRotation = false,  bool isModifyScale = false)
        {
            _endMoveCallback = onMoveEnded ?? (() => { }) ;
            Move(to, finishParent, isModifyRotation, isModifyScale);
        }

        public void Move(Transform to, Transform finishParent = null, bool isModifyRotation = false,  bool isModifyScale = false)
        { 
            _target = to;

            if (finishParent)
            {
                _finalParent = finishParent;
                _moving += Rotate;
            }
            
            if (_isCanChangeScale && isModifyScale)
            {
                _scaleModifier.Init(transform, GetDistanceToTarget(), _scaleCurve);
                _moving += UpdateScale;
            }
            
            _isModifyRotation = isModifyRotation;
            transform.Unparent();
            enabled = true;
            Started.Invoke();
        }

        protected override void Run()
        {
            float distance = GetDistanceToTarget();
            
            _moving.Invoke(distance);

            if (IsFinished(distance))
                FinishTranslation();
        }

        private void Rotate(float distanceToTarget)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, GetFinalRotation(),
                Time.deltaTime * _speed / distanceToTarget);
        }

        private void LerpTranslate(float distance)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                Target.position,
                _speed * Time.smoothDeltaTime / Mathf.Pow(distance, 1f - _power));
        }
        
        private void Translate(float _)
        {
            Vector3 translateDirection = (Target.position - transform.position).normalized;
            Vector3 deltaTranslation = translateDirection * _speed * Time.smoothDeltaTime;
            transform.Translate(deltaTranslation, Space.World);
        }

        private void FinishTranslation()
        {
            enabled = false;
            transform.position = _target.position;
            
            transform.SetParent(_finalParent, true);

            if (_finalParent)
            {
                _moving -= Rotate;
                transform.rotation = GetFinalRotation();
            }

            if (_isCanChangeScale)
                _moving -= UpdateScale;

            Ended.Invoke();
            _endMoveCallback.Invoke();
            _endMoveCallback = () => { };
        }

        private bool IsFinished(float distance) =>
            distance < _errorOffset;

        private Quaternion GetFinalRotation() =>
            _isModifyRotation
                ? _finalParent.rotation * Quaternion.Euler(_rotationOffset)
                : _finalParent.rotation;

        private float GetDistanceToTarget() =>
            Vector3.Distance(transform.position, Target.position);

        private void UpdateScale(float distanceToTarget) =>
            _scaleModifier.UpdateScale(distanceToTarget);
    }
}