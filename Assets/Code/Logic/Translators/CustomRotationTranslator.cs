using UnityEngine;

namespace Logic.Translators
{
    public class CustomRotationTranslator : CustomVector3Translatable
    {
        protected override void ApplyTranslation(Vector3 value)
        {
            transform.rotation = Quaternion.Euler(value);
        }
    }
}