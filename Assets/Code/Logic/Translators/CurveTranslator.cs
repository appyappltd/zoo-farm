using UnityEngine;

namespace Logic.Translators
{
    public class CurveTranslator : Translator
    {
        [SerializeField] private AnimationCurve _curve;

        // protected override Vector3 UpdatePosition(Vector3 from, Vector3 to, float delta)
        // {
        //     float curveDelta = _curve.Evaluate(delta);
        //     return Vector3.Lerp(from, to, curveDelta);
        // }
        public CurveTranslator(ITranslatable translatable) : base(translatable)
        {
        }
    }
}