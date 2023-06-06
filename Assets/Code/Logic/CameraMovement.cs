using NTC.Global.Cache;
using UnityEngine;

namespace Logic
{
    public class CameraMovement : MonoCache
    {
        [SerializeField] private float smoothing;

        private Transform _target;
        private Transform _parentTransform;

        public void Construct(Transform target)
        {
            _target = target;
            _parentTransform = transform.parent;
            enabled = true;
        }

        protected override void LateRun()
        {
            _parentTransform.position = Vector3.Lerp(
                _parentTransform.position,
                _target.position,
                smoothing * Time.smoothDeltaTime);
        }
    }
}