using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class Translatable<T> : MonoCache, ITranslatableInit<T>
    {
        private const float FinalTranslateValue = 1;

        [SerializeField] private float _speed;
        [SerializeField] private bool _isPreload;

        [ShowIf("_isPreload")]
        [SerializeField] private T _from;
        [ShowIf("_isPreload")]
        [SerializeField] private T _to;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<T, float, T> _valueModifiers = (value, delta) => value;
        private Func<T, T, float, T> _valueLerp;

        private float _delta;

        private bool IsComplete => _delta >= FinalTranslateValue;
        public bool IsPreload => _isPreload;

        public event Action<ITranslatable> Begin = t => {};
        public event Action<ITranslatable> End = t => {};
        
        protected abstract void OnInit();
        protected abstract void ApplyTranslation(T value);
        protected abstract void SetValueLerp(ref Func<T, T, float, T> valueLerp);
        
        private void Awake()
        {
            enabled = false;
        }

        public void Init()
        {
            if (_isPreload == false)
            {
                throw new Exception("Preload is not enabled");
            }
            
            _delta = 0;
            SetValueLerp(ref _valueLerp);
            OnInit();
            Begin.Invoke(this);
        }
        
        public void Init(T from, T to)
        {
            if (_isPreload)
            {
                throw new Exception("You are trying to set the settings that are predefined");
            }
            
            _delta = 0;
            _from = from;
            _to = to;
            SetValueLerp(ref _valueLerp);
            OnInit();
            Begin.Invoke(this);
        }

        public void Enable() =>
            gameObject.Enable();

        public void Disable() =>
            gameObject.Disable();

        public bool TryUpdate()
        {
            if (IsComplete)
            {
                End.Invoke(this);
                return false;
            }

            float delta = UpdateDelta();
            delta = _deltaModifiers.Invoke(delta);
            T value = _valueLerp.Invoke(_from, _to, delta);
            // value = _valueModifiers.Invoke(value, delta);

            for (int index = 0; index < _valueModifiers.GetInvocationList().Length; index++)
            {
                Delegate @delegate = _valueModifiers.GetInvocationList()[index];
                Func<T, float, T> variable = (Func<T, float, T>) @delegate;
                value = variable.Invoke(value, delta);
            }

            ApplyTranslation(value);
            return true;
        }

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