using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Movement
{
    public class Aligner : MonoCache
    {
        private const float TurnOffThreshold = 1f;

        [SerializeField] private float _rotateSpeed;

        private Quaternion _aligneRotation;

        protected override void Run() =>
            Rotate();

        public void Aligne(Quaternion toRotation)
        {
            _aligneRotation = toRotation.normalized;
            enabled = true;
        }

        private void Rotate()
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _aligneRotation,
                _rotateSpeed * Time.smoothDeltaTime);

            if (IsAligned())
            {
                transform.rotation = _aligneRotation;
                enabled = false;
            }
        }

        private bool IsAligned()
        {
            float dot = Vector3.Dot(transform.forward, _aligneRotation * Vector3.forward);
            return Mathf.Approximately(dot, TurnOffThreshold);
        }
    }
}