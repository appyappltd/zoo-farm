using NaughtyAttributes;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Logic.Translators
{
    public class TranslatorConstructor : MonoCache
    {
        [SerializeField] private AnimationCurve _deltaTimeCurve;
        [SerializeField] private AnimationCurve _vectorCurve;
        
        [SerializeField] [RequireInterface(typeof(ITranslatable))] private MonoBehaviour _translatable;
        [SerializeField] private float _speed;
        
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        
        private ITranslator _translator;
        
        private void Awake()
        {
            enabled = false;
            _translator = new Translator();
            _translator.SetPositionLerp(Vector3.LerpUnclamped);
            _translator.AddDeltaModifier(_deltaTimeCurve.Evaluate);
        }

        [Button("Translate")]
        private void Translate()
        {
            _translator.Translate((ITranslatable) _translatable, _from.position, _to.position, _speed);
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