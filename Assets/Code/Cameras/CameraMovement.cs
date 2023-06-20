using System;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cameras
{
    public class CameraMovement : MonoCache
    {
        [FormerlySerializedAs("smoothing")] [SerializeField] private float _smoothing;

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
            parentTransform.position = Vector3.Lerp(
                parentTransform.position,
                toPosition,
                _smoothing * Time.smoothDeltaTime);
        }
        
        private void FollowToTarget()
        {
            parentTransform.position = Vector3.Lerp(
                parentTransform.position,
                followTarget.position,
                _smoothing * Time.smoothDeltaTime);
        }
    }
}