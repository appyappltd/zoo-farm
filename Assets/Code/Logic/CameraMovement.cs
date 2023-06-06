using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic
{
    public class CameraMovement : MonoCache
    {
        [FormerlySerializedAs("distance")] [SerializeField] private float _distance = 20;
        [FormerlySerializedAs("cameraOffset")] [SerializeField] private Vector3 _cameraOffset;
        [SerializeField] private float _smoothing;

        private Transform _target;
        private Vector3 offset;

        private void Start() =>
            offset = transform.rotation * Vector3.back * _distance;

        public void Construct(Transform target)
        {
            _target = target;
            enabled = true;
        }

        protected override void LateRun()
        {
            Vector3 targetCamPos = _target.position + offset + _cameraOffset;
            transform.position = Vector3.Lerp(transform.position,
                targetCamPos,
                _smoothing * Time.deltaTime);
        }
    }
}
