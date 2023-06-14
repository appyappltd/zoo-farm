using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslator
    {
        public void AddTranslatable(ITranslatable translatable, Vector3 from, Vector3 to);
    }
}