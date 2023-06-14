using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslatable
    {
        void Init(Vector3 from, Vector3 to);
        bool TryUpdate();
    }
}