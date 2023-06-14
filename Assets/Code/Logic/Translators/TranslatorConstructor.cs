using NaughtyAttributes;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Logic.Translators
{
    public class TranslatorConstructor : MonoCache
    {
        [SerializeField] private AnimationCurve _curve;
        
        [SerializeField] [RequireInterface(typeof(ITranslatable))] private MonoBehaviour _translatable;
        [SerializeField] private float _speed;
        
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        
        private ITranslator _translator;
        
        private void Awake()
        {
            enabled = false;
            _translator = new CustomTranslator((ITranslatable) _translatable, _curve);
            _translator.SetDeltaModifier(_curve.Evaluate);
            _translator.SetPositionModifier(Vector3.LerpUnclamped);
        }

        [Button("Translate")]
        private void Translate()
        {
            _translator.Translate(_from.position, _to.position, _speed);
            enabled = true;
        }

        protected override void Run()
        {
            if (_translator.TryUpdate() == false)
            {
                enabled = false;
            }
        }
    }
}