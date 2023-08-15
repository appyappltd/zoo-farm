using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Bubble
{
    public class MovingBubble : MonoCache
    {
        private const float ToCameraOffset = 3f;
        
        [SerializeField] private Transform _parent;

        private Transform _cameraTransform;
        private Vector3 _offset;

        private void Awake()
        {
            _cameraTransform = Camera.main.transform;
            _offset = transform.localPosition + _cameraTransform.forward * ToCameraOffset;
            transform.LookAt(_cameraTransform);
        }

        protected override void LateRun()
        {
            transform.position = _parent.position + _offset;
            transform.LookAt(_cameraTransform);
        }
    }
}
