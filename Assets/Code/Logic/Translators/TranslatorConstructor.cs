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
            
            Translator.AddTranslatable(Translatables[_index], _from.position, _to.position);
            _index++;
        }
    }
}