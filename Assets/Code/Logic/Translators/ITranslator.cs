using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslator
    {
        public void Translate(Vector3 from, Vector3 to);
    }
}