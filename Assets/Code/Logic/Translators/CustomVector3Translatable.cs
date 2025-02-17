using System;
using Tools.Extension;
using UnityEngine;

namespace Logic.Translators
{
    public abstract class CustomVector3Translatable : Translatable<Vector3>
    {
        [SerializeField] private AnimationCurve _timeCurve;
        [SerializeField] private AnimationCurve _deltaXCurve;
        [SerializeField] private AnimationCurve _deltaYCurve;
        [SerializeField] private AnimationCurve _deltaZCurve;

        protected override void ApplyModifiers()
        {
            AddDeltaModifier(_timeCurve.Evaluate);
            AddPositionModifier((vector, delta) => vector.ChangeX(vector.x * _deltaXCurve.Evaluate(delta)));
            AddPositionModifier((vector, delta) => vector.ChangeY(vector.y * _deltaYCurve.Evaluate(delta)));
            AddPositionModifier((vector, delta) => vector.ChangeZ(vector.z * _deltaZCurve.Evaluate(delta)));
        }
        
        protected override void SetValueLerp(ref Func<Vector3, Vector3, float, Vector3> valueLerp)
        {
            valueLerp = Vector3.LerpUnclamped;
        }
    }
}