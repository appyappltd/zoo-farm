using UnityEngine;

namespace Logic.Translators
{
    public class LinearPositionTranslatable : Translatable<Vector3>
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