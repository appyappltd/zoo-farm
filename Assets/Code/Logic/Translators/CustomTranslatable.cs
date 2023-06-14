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
            enabled = true;
        }

        protected override void ApplyTranslation(Vector3 vector)
        {
            transform.position = vector;
        }

        protected override void Run()
        {
            UpdateToPosition(ToTransform.position);
        }
    }
}