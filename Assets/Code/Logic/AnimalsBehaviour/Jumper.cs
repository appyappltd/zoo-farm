using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class Jumper : MonoCache
    {
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private Transform _transform;
        [SerializeField] private AnimationCurve _moveY;
        [SerializeField] private AnimationCurve _moveZ;
        [SerializeField] private float _duration;
        
        private float _elapsedTime;
        private Vector3 _startPosition;

        public event Action Jumped = () => { };
        public event Action StartJump = () => { };

        [Button("Jump")]
        public void Jump()
        {
            _elapsedTime = 0;
            enabled = true;
            _animator.SetJump();
            _startPosition = _transform.position;
            StartJump.Invoke();
        }
        
        protected override void Run()
        {
            _elapsedTime += Time.deltaTime;
            float curveTime = _elapsedTime / _duration;

            float newY = _moveY.Evaluate(curveTime) * _startPosition.y - _startPosition.y;
            float newZ = _moveZ.Evaluate(curveTime);
            
            Vector3 newPosition = _startPosition + transform.TransformDirection(new Vector3(0, newY, newZ));
            _transform.position = newPosition;

            if (_elapsedTime >= _duration)
            {
                enabled = false;
                Jumped.Invoke();
            }
        }
    }
}