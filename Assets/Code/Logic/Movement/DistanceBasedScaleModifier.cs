using UnityEngine;

namespace Logic.Movement
{
    public class DistanceBasedScaleModifier
    {
        private Transform _modificationTarget;
        private float _initialDistance;
        private AnimationCurve _curve;

        private Vector3 _initialScale;

        public void Init(Transform modificationTarget, float initialDistance, AnimationCurve curve)
        {
            _curve = curve;
            _initialDistance = initialDistance;
            _modificationTarget = modificationTarget;
            _initialScale = modificationTarget.localScale;
        }

        public void UpdateScale(float byDistance) =>
            _modificationTarget.localScale = _initialScale * _curve.Evaluate(byDistance / _initialDistance);
    }
}