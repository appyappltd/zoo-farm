using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class Translator : MonoCache, ITranslator
    {
        private const float FinalTranslateValue = 1;
        
        private ITranslatable _translatable;
        private Vector3 _toPosition;
        private float _delta;
        private float _distance;

        [SerializeField] private float _speed;
        protected Vector3 From { get; private set; }
        protected Vector3 To { get; private set; }

        protected abstract void Init(ITranslatable translatable);
        protected abstract Vector3 UpdatePosition(float delta);

        public void Translate(Vector3 from, Vector3 to)
        {
            From = from;
            To = to;
            enabled = true;
        }

        protected override void Run()
        {
            UpdateDelta();
            _translatable.Warp(UpdatePosition(_delta));
            CheckIsComplete();
        }

        private void UpdateDelta() =>
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime / _distance);
        
        private void CheckIsComplete()
        {
            if (_delta >= FinalTranslateValue)
                enabled = false;
        }
    }
}