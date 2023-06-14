using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class Translatable : MonoCache, ITranslatable
    {
        private const float FinalTranslateValue = 1;
        
        [SerializeField] private float _speed;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<Vector3, float, Vector3> _positionModifiers = (vector3, delta) => vector3;
        private Func<Vector3, Vector3, float, Vector3> _positionLerp = Vector3.LerpUnclamped;
        
        private float _delta;
        private Vector3 _from;
        private Vector3 _to;

        private bool IsComplete => _delta >= FinalTranslateValue;

        protected abstract void OnInit();

        protected abstract void ApplyTranslation(Vector3 vector);

        protected void UpdateToPosition(Vector3 newToPosition) =>
            _to = newToPosition;

        public void Init(Vector3 from, Vector3 to)
        {
            _delta = 0;
            _from = from;
            _to = to;
            OnInit();
        }

        public bool TryUpdate()
        {
            if (IsComplete)
                return false;

            float delta = UpdateDelta();
            delta = _deltaModifiers.Invoke(delta);
            Vector3 position = _positionLerp.Invoke(_from, _to, delta);
            position = _positionModifiers.Invoke(position, delta);
            ApplyTranslation(position);
            return true;
        }

        protected void SetPositionLerp(Func<Vector3, Vector3, float, Vector3> func) =>
            _positionLerp += func ?? throw new NullReferenceException();

        protected void AddDeltaModifier(Func<float, float> func) =>
            _deltaModifiers += func ?? throw new NullReferenceException();

        protected void AddPositionModifier(Func<Vector3, float, Vector3> func) =>
            _positionModifiers += func ?? throw new NullReferenceException();

        private float UpdateDelta()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime);
            return _delta;
        }
    }
}