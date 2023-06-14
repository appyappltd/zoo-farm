using System;
using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslator
    {
        public void Translate(Vector3 from, Vector3 to, float speed);
        public bool TryUpdate();
        void SetDeltaModifier(Func<float, float> func);
        void SetPositionModifier(Func<Vector3, Vector3, float, Vector3> func);
    }
}