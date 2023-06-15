using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tools;
using UnityEngine;

namespace Logic.Translators
{
    public class TranslatableAgent : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(ITranslatable))] private MonoBehaviour _mainTranslatable;
        [SerializeField] [RequireInterface(typeof(List<ITranslatable>))] private List<MonoBehaviour> _subTranslatables;

        private ReadOnlyCollection<ITranslatable> _translatables;
        
        public ReadOnlyCollection<ITranslatable> SubTranslatables => _translatables;
        public ITranslatable MainTranslatable => (ITranslatable) _mainTranslatable;
        
        private void Awake()
        {
            _translatables = new ReadOnlyCollection<ITranslatable>(_subTranslatables.Cast<ITranslatable>().ToList());
        }
    }
}