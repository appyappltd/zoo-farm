using UnityEngine;

namespace Logic.Translators
{
    public class SlerpTranslator : Translator
    {
        // protected override Vector3 UpdatePosition(Vector3 from, Vector3 to, float delta)
        // {
        //     return Vector3.Slerp(from, to, delta);
        // }
        public SlerpTranslator(ITranslatable translatable) : base()
        {
        }
    }
}