using System.Collections.Generic;
using NTC.Global.Cache;

namespace Logic.Translators
{
    public abstract class Translator: MonoCache, ITranslator
    {
        private readonly List<ITranslatable> _translatables = new List<ITranslatable>();

        private ITranslatable _translatable;

        private void OnValidate() =>
            enabled = false;

        public void AddTranslatable(ITranslatable translatable)
        {
            if (_translatables.Contains(translatable))
                return;

            _translatables.Add(translatable);
            enabled = true;
        }

        public void RemoveTranslatable(ITranslatable translatable)
        {
            _translatables.Remove(translatable);

            if (_translatables.Count <= 0)
                enabled = false;
        }

        protected void UpdateTranslatable()
        {
            for (var index = 0; index < _translatables.Count; index++)
            {
                ITranslatable translatable = _translatables[index];

                if (translatable.TryUpdate() == false)
                {
                    RemoveTranslatable(translatable);
                }
            }
        }
    }
}