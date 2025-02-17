using UnityEngine;

namespace Logic.Translators
{
    public class CustomPositionTranslatable : CustomVector3Translatable
    {
        protected override void ApplyTranslation(Vector3 value)
        {
            transform.localPosition = value;
        }
    }
}