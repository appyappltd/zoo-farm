using NTC.Global.Cache;
using Tools.Constants;
using UnityEngine;

namespace Logic
{
    public class SineAnimate : MonoCache
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private float _speed = 2;
        [SerializeField] private float _min = 1.5f;
        [SerializeField] private float _max = 2;
        
        private Vector2 _startScale;

        private void Awake()
        {
            _startScale = _sprite.size;
            _sprite.drawMode = SpriteDrawMode.Sliced;
        }

        protected override void Run()
        {
            float sine = Mathf.Sin(Time.time * _speed);
            float scaleModifier = (sine + 1) * Arithmetic.ToHalf;
            _sprite.size = _startScale * Mathf.Lerp(_min, _max, scaleModifier);
        }
    }
}