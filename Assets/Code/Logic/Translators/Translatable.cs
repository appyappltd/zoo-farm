using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class Translatable<T> : MonoCache, ITranslatable
    {
        private const float FinalTranslateValue = 1;

        [SerializeField] private float _speed;
        private T _from;
        private T _to;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<T, float, T> _valueModifiers = (value, delta) => value;
        private Func<T, T, float, T> _valueLerp;

        private float _delta;

        private bool IsComplete => _delta >= FinalTranslateValue;

        protected abstract void OnInit();

        protected abstract void ApplyTranslation(T vector);
        protected abstract void SetValueLerp(ref Func<T, T, float, T> valueLerp);

        protected void UpdateToPosition(T newToPosition) =>
            _to = newToPosition;

        private void Awake()
        {
            enabled = false;
        }

        public void Init(T from, T to)
        {
            _delta = 0;
            _from = from;
            _to = to;
            SetValueLerp(ref _valueLerp);
            OnInit();
        }

        public bool TryUpdate()
        {
            if (IsComplete)
                return false;

            float delta = UpdateDelta();
            delta = _deltaModifiers.Invoke(delta);
            T value = _valueLerp.Invoke(_from, _to, delta);
            value = _valueModifiers.Invoke(value, delta);
            ApplyTranslation(value);
            return true;
        }

        protected void SetPositionLerp(Func<T, T, float, T> func) =>
            _valueLerp += func ?? throw new NullReferenceException();

        protected void AddDeltaModifier(Func<float, float> func) =>
            _deltaModifiers += func ?? throw new NullReferenceException();

        protected void AddPositionModifier(Func<T, float, T> func) =>
            _valueModifiers += func ?? throw new NullReferenceException();

        private float UpdateDelta()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime);
            return _delta;
        }
    }
}