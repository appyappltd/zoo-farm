using System;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class Translatable : MonoCache, ITranslatable
    {
        private const float FinalTranslateValue = 1;

        protected Transform ToTransform;

        [SerializeField] private float _speed;
        private Vector3 _from;
        private Vector3 _to;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<Vector3, float, Vector3> _vectorModifiers = (vector3, delta) => vector3;
        private Func<Vector3, Vector3, float, Vector3> _vectorLerp = Vector3.LerpUnclamped;

        private float _delta;

        private bool IsComplete => _delta >= FinalTranslateValue;

        protected abstract void OnInit();

        protected abstract void ApplyTranslation(Vector3 vector);

        protected void UpdateToPosition(Vector3 newToPosition) =>
            _to = newToPosition;

        private void Awake()
        {
            enabled = false;
        }

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
            Vector3 vector = _vectorLerp.Invoke(_from, _to, delta);
            vector = _vectorModifiers.Invoke(vector, delta);
            ApplyTranslation(vector);
            return true;
        }

        protected void SetPositionLerp(Func<Vector3, Vector3, float, Vector3> func) =>
            _vectorLerp += func ?? throw new NullReferenceException();

        protected void AddDeltaModifier(Func<float, float> func) =>
            _deltaModifiers += func ?? throw new NullReferenceException();

        protected void AddPositionModifier(Func<Vector3, float, Vector3> func) =>
            _vectorModifiers += func ?? throw new NullReferenceException();

        private float UpdateDelta()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime);
            return _delta;
        }
    }
}