using UnityEngine;

namespace Logic.Translators
{
    public class LinearPositionTranslatable : Translatable
    {
        protected override void OnInit()
        {
        }

        protected override void ApplyTranslation(Vector3 vector)
        {
            transform.position = vector;
        }
    }
}