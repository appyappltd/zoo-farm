using System;
using UnityEngine;

namespace Logic.Translators
{
    public class Translator : ITranslator
    {
        private const float FinalTranslateValue = 1;
        
        private readonly ITranslatable _translatable;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<Vector3, Vector3, float, Vector3> _positionModifiers = Vector3.Lerp;

        private Vector3 _toPosition;
        private float _delta;

        private float _speed;

        private Vector3 _from;
        private Vector3 _to;

        private bool IsComplete => _delta >= FinalTranslateValue;
        
        protected Translator(ITranslatable translatable)
        {
            _translatable = translatable;
        }

        public void SetDeltaModifier(Func<float, float> func) =>
            _deltaModifiers = func ?? throw new NullReferenceException();

        public void SetPositionModifier(Func<Vector3, Vector3, float, Vector3> func) =>
            _positionModifiers = func ?? throw new NullReferenceException();

        public void Translate(Vector3 from, Vector3 to, float speed)
        {
            _delta = 0;
            _from = from;
            _to = to;
            _speed = speed;
        }

        public bool TryUpdate()
        {
            if (IsComplete)
                return false;

            UpdateDelta();
            float delta = _deltaModifiers.Invoke(_delta);
            Vector3 position = _positionModifiers.Invoke(_from, _to, delta);
            _translatable.Warp(position);
            return true;
        }

        private void UpdateDelta() =>
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime);
    }
}