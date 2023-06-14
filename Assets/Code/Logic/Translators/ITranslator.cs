using System;
using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslator
    {
        public void Translate(ITranslatable translatable, Vector3 from, Vector3 to, float speed);
        public bool TryUpdate();
        void AddDeltaModifier(Func<float, float> func);
        void SetPositionLerp(Func<Vector3, Vector3, float, Vector3> func);
        void AddPositionModifier(Func<Vector3, float, Vector3> func);
    }
}