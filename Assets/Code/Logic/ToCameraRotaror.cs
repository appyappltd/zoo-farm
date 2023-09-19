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
            ShiftPosition();
            LateRun();
        }

        public void UpdateRotation() =>
            LateRun();

        protected override void LateRun() =>
            transform.forward = _cameraTransform.forward;

        private void ShiftPosition()
        {
            foreach (Transform child in transform)
            {
                if (child.IsChildOf(transform))
                    child.Translate(-Vector3.forward * _toCameraOffset, Space.Self);
            }
        }
    }
}