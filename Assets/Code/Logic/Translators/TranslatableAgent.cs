using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AYellowpaper;
using NaughtyAttributes;
using Services.Pools;
using UnityEngine;

namespace Logic.Translators
{
    public class TranslatableAgent : MonoBehaviour , IPoolable
    {
        [SerializeField] private bool _isAutoPlay;
        
        [ShowIf("_isAutoPlay")]
        [SerializeField] [RequireInterface(typeof(ITranslator))] private MonoBehaviour _translator;
        [SerializeField] [RequireInterface(typeof(ITranslatable))] private MonoBehaviour _mainTranslatable;
        [SerializeField] [RequireInterface(typeof(ITranslatable))] private List<MonoBehaviour> _subTranslatables;

        private ReadOnlyCollection<ITranslatable> _translatables;
        
        public ReadOnlyCollection<ITranslatable> Sub => _translatables;
        public ITranslatable Main => (ITranslatable) _mainTranslatable;
        
        GameObject IPoolable.GameObject => gameObject;

        private void Awake()
        {
            _translatables = new ReadOnlyCollection<ITranslatable>(_subTranslatables.Cast<ITranslatable>().ToList());

            if (_isAutoPlay)
            {
                ITranslator translator = (ITranslator) _translator;
                translator.Add(Main);

                foreach (ITranslatable translatable in Sub)
                {
                    translator.Add(translatable);
                }
            }
        }
    }
}