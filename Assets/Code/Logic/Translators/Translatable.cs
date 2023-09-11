using System;
using NaughtyAttributes;
using NTC.Global.Cache;
using NTC.Global.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Translators
{
    public abstract class Translatable<T> : MonoCache, ITranslatableParametric<T>
    {
        private const float FinalTranslateValue = 1;

        [SerializeField] private float _speed;
        [SerializeField] private bool _isPreload;
        [FormerlySerializedAs("_looped")] [SerializeField] private bool _isLooped;

        [ShowIf("_isPreload")]
        [SerializeField] private T _from;
        [ShowIf("_isPreload")]
        [SerializeField] private T _to;

        private Func<float, float> _deltaModifiers = f => f;
        private Func<T, float, T> _valueModifiers = (value, delta) => value;
        private Func<T, T, float, T> _valueLerp;

        private float _delta;
        private bool _isLastCycle;

        private bool IsComplete => _delta >= FinalTranslateValue;
        public bool IsPreload => _isPreload;

        public event Action<ITranslatable> Begin = _ => {};
        public event Action<ITranslatable> End = _ => {};
        
        protected abstract void ApplyModifiers();
        protected abstract void ApplyTranslation(T value);
        protected abstract void SetValueLerp(ref Func<T, T, float, T> valueLerp);
        
        private void Awake() =>
            Init();

        private void Init()
        {
            SetValueLerp(ref _valueLerp);
            ApplyModifiers();
        }

        public void Play()
        {
            if (_isPreload == false)
                throw new Exception("Preload is not enabled");

            _isLastCycle = false;
            Restart();
        }

        public void Stop(bool atCycleEnd)
        {
            if (IsComplete)
                return;
            
            _isLastCycle = true;
            
            if (atCycleEnd)
                return;

            End = _ => { };
            _delta = FinalTranslateValue;
        }

        public void Play(T from, T to)
        {
            if (_isPreload)
                throw new Exception("You are trying to set the settings that are predefined");
            
            _from = from;
            _to = to;
            
            Restart();
        }

        public bool TryUpdate()
        {
            if (gameObject.activeSelf == false)
                return false;

            if (IsComplete)
            {
                End.Invoke(this);

                if (_isLooped == false || _isLastCycle)
                    return false;
                
                Restart();
                return true;
            }

            float delta = UpdateDelta();
            delta = _deltaModifiers.Invoke(delta);
            T value = _valueLerp.Invoke(_from, _to, delta);

            value = ModifyTranslation(value, delta);
            ApplyTranslation(value);
            return true;
        }

        public void ResetToDefault()
        {
            T value = _valueLerp.Invoke(_from, _to, 0);
            value = ModifyTranslation(value, 0);
            ApplyTranslation(value);
        }
        
        public void Enable() =>
            gameObject.Enable();

        public void Disable() =>
            gameObject.Disable();

        protected void AddDeltaModifier(Func<float, float> func) =>
            _deltaModifiers += func ?? throw new NullReferenceException();

        protected void AddPositionModifier(Func<T, float, T> func) =>
            _valueModifiers += func ?? throw new NullReferenceException();

        private float UpdateDelta()
        {
            _delta = Mathf.MoveTowards(_delta, FinalTranslateValue, _speed * Time.smoothDeltaTime);
            return _delta;
        }

        private T ModifyTranslation(T value, float delta)
        {
            for (int index = 0; index < _valueModifiers.GetInvocationList().Length; index++)
            {
                Delegate @delegate = _valueModifiers.GetInvocationList()[index];
                Func<T, float, T> variable = (Func<T, float, T>) @delegate;
                value = variable.Invoke(value, delta);
            }

            return value;
        }

        private void Restart()
        {
            _delta = 0;
            Begin.Invoke(this);
        }
    }
}