using UnityEngine;

namespace Logic.Translators
{
    public class CustomTranslator : Translator
    {
        public CustomTranslator(ITranslatable translatable, AnimationCurve curve) : base(translatable)
        {
            SetDeltaModifier(curve.Evaluate);
            SetPositionModifier(Vector3.LerpUnclamped);
        }
    }
}