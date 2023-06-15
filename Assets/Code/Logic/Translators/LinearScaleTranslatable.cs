using System;
using UnityEngine;

namespace Logic.Translators
{
    public class LinearScaleTranslatable : Translatable<Vector3>
    {
        protected override void OnInit()
        {
        }

        protected override void ApplyTranslation(Vector3 value)
        {
            transform.localScale = value;
        }

        protected override void SetValueLerp(ref Func<Vector3, Vector3, float, Vector3> valueLerp)
        {
            valueLerp = Vector3.Lerp;
        }
    }
}