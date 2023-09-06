using System;
using System.Collections.Generic;
using NTC.Global.Cache;

namespace Logic.Translators
{
    public abstract class Translator : MonoCache, ITranslator
    {
        private readonly List<ITranslatable> _translatables = new List<ITranslatable>();

        private ITranslatable _translatable;

        public bool IsActive => enabled;

        private void OnValidate() =>
            enabled = false;

        public void Add(ITranslatable translatable)
        {
            if (_translatables.Contains(translatable))
                throw new InvalidOperationException(nameof(translatable));

            _translatables.Add(translatable);
            enabled = true;
        }

        public void Remove(ITranslatable translatable)
        {
            if (_translatables.Contains(translatable) == false)
                throw new InvalidOperationException(nameof(translatable));
            
            _translatables.Remove(translatable);

            if (_translatables.Count <= 0)
                enabled = false;
        }

        public void RemoveAll()
        {
            _translatables.Clear();
            enabled = false;
        }
        
        protected void UpdateTranslatable()
        {
            for (var index = 0; index < _translatables.Count; index++)
            {
                ITranslatable translatable = _translatables[index];

                if (translatable.TryUpdate() == false)
                {
                    Remove(translatable);
                }
            }
        }
    }
}