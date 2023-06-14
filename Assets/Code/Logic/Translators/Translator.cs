using System.Collections.Generic;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class Translator : MonoCache, ITranslator
    {
        private readonly List<ITranslatable> _translatables = new List<ITranslatable>();

        private ITranslatable _translatable;

        private void Awake()
        {
            enabled = false;
        }

        public void AddTranslatable(ITranslatable translatable, Vector3 from, Vector3 to)
        {
            if (_translatables.Contains(translatable))
                return;

            enabled = true;
            translatable.Init(from, to);
            _translatables.Add(translatable);
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

        private void RemoveTranslatable(ITranslatable translatable)
        {
            _translatables.Remove(translatable);

            if (_translatables.Count <= 0)
                enabled = false;
        }
    }
}