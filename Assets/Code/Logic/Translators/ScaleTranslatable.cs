using UnityEngine;

namespace Logic.Translators
{
    public class ScaleTranslatable : Translatable
    {
        protected override void OnInit()
        {
        }

        protected override void ApplyTranslation(Vector3 vector)
        {
            transform.localScale = vector;
        }
    }
}