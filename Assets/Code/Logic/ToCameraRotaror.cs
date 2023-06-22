using NTC.Global.Cache;
using UnityEngine;

namespace Logic
{
    public class ToCameraRotator : MonoCache
    {
        private Transform _cameraTransform;

        private void Awake() =>
            _cameraTransform = Camera.main.transform;

        protected override void LateRun() =>
            transform.forward = _cameraTransform.forward;
    }
}