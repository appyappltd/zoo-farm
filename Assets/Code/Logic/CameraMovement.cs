using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic
{
    public class CameraMovement : MonoCache
    {
        [FormerlySerializedAs("smoothing")] [SerializeField] private float _smoothing;

        private Transform target;
        private Transform parentTransform;

        public void Construct(Transform target)
        {
            this.target = target;
            parentTransform = transform.parent;
            enabled = true;
        }

        protected override void LateRun()
        {
            parentTransform.position = Vector3.Lerp(
                parentTransform.position,
                target.position,
                _smoothing * Time.smoothDeltaTime);
        }
    }
}