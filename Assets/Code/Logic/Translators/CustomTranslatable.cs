using Tools.Extension;
using UnityEngine;

namespace Logic.Translators
{
    public class CustomTranslatable : Translatable
    {
        [SerializeField] private AnimationCurve _timeCurve;
        [SerializeField] private AnimationCurve _deltaYCurve;

        protected override void OnInit()
        {
            AddDeltaModifier(_timeCurve.Evaluate);
            AddPositionModifier(((vector, delta) => vector.ChangeY(vector.y + _deltaYCurve.Evaluate(delta))));
        }

        protected override void Run()
        {
            UpdateToPosition(transform.position);
        }
    }
}