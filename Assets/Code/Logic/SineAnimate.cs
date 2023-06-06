using NTC.Global.Cache;
using Tools.Constants;
using UnityEngine;

namespace Logic
{
    public class SineAnimate : MonoCache
    {
        private const float Offset = 0.5f;

        [SerializeField] private float _speed = 2;
        [SerializeField] private float _min = 1.5f;
        [SerializeField] private float _max = 2;
        [SerializeField] private Vector3 _movement;
        [SerializeField] private Vector3 _scale = new(1, 1, 0);

        private float t;
        private Vector3 startPos;
        private Vector3 startScale;

        private void Start()
        {
            var selfTransform = transform;
            startPos = selfTransform.localPosition;
            startScale = selfTransform.localScale;
        }

        protected override void Run()
        {
            t = (t + Time.deltaTime * _speed) % Trigonometry.TwoPiGrade;
            var k = _min + (Mathf.Sin(t) * Arithmetic.ToHalf + Offset) * (_max - _min);
            transform.localPosition = startPos + _movement * k;
            transform.localScale = new Vector3(
                Mathf.Lerp(startScale.x, k, _scale.x),
                Mathf.Lerp(startScale.y, k, _scale.y),
                Mathf.Lerp(startScale.z, k, _scale.z));
        }
    }
}