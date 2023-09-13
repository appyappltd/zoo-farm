using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Cameras
{
    public class CameraMovement : MonoCache
    {
        [SerializeField] private float _smoothing;
        [SerializeField] private float _maxSpeed;
        [SerializeField] [Min(0f)] [MaxValue(1f)] private float _power = 0.5f;

        private Transform followTarget;
        private Transform parentTransform;
        private Vector3 toPosition;
        private Action moveAction;

        private void Awake()
        {
            parentTransform = transform.parent;
            enabled = true;
        }

        protected override void LateRun() =>
            moveAction.Invoke();

        public void MoveTo(Vector3 position)
        {
            toPosition = position;
            moveAction = MoveToPosition;
        }

        public void Follow(Transform target)
        {
            followTarget = target;
            moveAction = FollowToTarget;
        }

        private void MoveToPosition()
        {
            parentTransform.position =
                Vector3.Lerp(
                parentTransform.position,
                toPosition,
                _smoothing * Time.smoothDeltaTime / Mathf.Pow(Vector3.Distance(parentTransform.position, toPosition),  1f - _power));
        }
        
        private void FollowToTarget()
        {
            parentTransform.position = Vector3.Lerp(
                parentTransform.position,
                followTarget.position,
                _smoothing * Time.smoothDeltaTime / Mathf.Pow(Vector3.Distance(parentTransform.position, followTarget.position), 1f - _power));
        }
    }
}