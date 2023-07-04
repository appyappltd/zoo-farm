using UnityEngine;

namespace Logic.Translators
{
    public class LocalLinearPositionTranslatable : LinearPositionTranslatable
    {
        protected override void ApplyTranslation(Vector3 value)
        {
            transform.localPosition = value;
        }
    }
}