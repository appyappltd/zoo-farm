using System;
using UnityEngine;

namespace Logic.Translators
{
    public class Translator : ITranslator
    {
        private const float FinalTranslateValue = 1;
        
        private ITranslatable _translatable;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<Vector3, float, Vector3> _positionModifiers = (vector3, delta) => vector3;
        private Func<Vector3, Vector3, float, Vector3> _positionLerp = Vector3.Lerp;

        private Vector3 _toPosition;
        private float _delta;

        private float _speed;

        private Vector3 _from;
        private Vector3 _to;
        private ITranslator _translatorImplementation;

        private bool IsComplete => _delta >= FinalTranslateValue;

        public void SetPositionLerp(Func<Vector3, Vector3, float, Vector3> func) =>
            _positionLerp += func ?? throw new NullReferenceException();

        public void AddDeltaModifier(Func<float, float> func) =>
            _deltaModifiers += func ?? throw new NullReferenceException();

        public void AddPositionModifier(Func<Vector3, float, Vector3> func) =>
            _positionModifiers += func ?? throw new NullReferenceException();

        public void Translate(ITranslatable translatable, Vector3 from, Vector3 to, float speed)
        {
            _translatable = translatable;
            _delta = 0;
            _from = from;
            _to = to;
            _speed = speed;
        }

        public bool TryUpdate()
        {
            if (IsComplete)
                return false;

            float delta = UpdateDelta();
            delta = _deltaModifiers.Invoke(delta);
            Vector3 position = _positionLerp.Invoke(_from, _to, delta);
            position = _positionModifiers.Invoke(position, delta);
            _translatable.Warp(position);
            return true;
        }

        private float UpdateDelta()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime);
            return _delta;
        }
    }
}