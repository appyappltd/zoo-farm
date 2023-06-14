using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Logic.Translators
{
    public class TranslatorConstructor : MonoCache
    {
        [SerializeField] [RequireInterface(typeof(ITranslator))] private MonoBehaviour _translator;
        [SerializeField] [RequireInterface(typeof(List<ITranslatable>))] private List<MonoBehaviour> _translatables;

        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        
        [SerializeField] private Vector3 _beginScale;
        [SerializeField] private Vector3 _endScale;

        private int _index;
        
        private ITranslator Translator => (ITranslator) _translator;
        private List<ITranslatable> Translatables => _translatables.Cast<ITranslatable>().ToList();

        [Button("Translate")]
        private void Translate()
        {
            if (_index == Translatables.Count)
            {
                _index = 0;
            }


            ITranslatable translatable = Translatables[_index];
            
            if (translatable is CustomScaleTranslatable scaleTranslatable)
            {
                scaleTranslatable.Init(_beginScale, _endScale);
            }

            if (translatable is CustomPositionTranslatable positionTranslatable)
            {
                positionTranslatable.Init(_from.position, _to.position);
            }
            
            Translator.AddTranslatable(translatable);
            _index++;
        }
    }
}