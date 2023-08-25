using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic
{
    public class ToCameraRotator : MonoCache
    {
        [SerializeField] private float _toCameraOffset = 3f;

        private Transform _cameraTransform;

        [Button("Rotate")]
        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            LateRun();
            ShiftPosition();
        }

        public void UpdateRotation() =>
            LateRun();

        protected override void LateRun() =>
            transform.forward = _cameraTransform.forward;

        private void ShiftPosition() =>
            transform.position -= _cameraTransform.forward * _toCameraOffset;
    }
}