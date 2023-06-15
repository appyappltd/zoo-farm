using UnityEngine;

namespace Logic.Translators
{
    public class CustomScaleTranslatable : CustomVector3Translatable
    {
        protected override void ApplyTranslation(Vector3 value)
        {
            transform.localScale = value;
        }
    }
}