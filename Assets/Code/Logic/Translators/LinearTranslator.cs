using UnityEngine;

namespace Logic.Translators
{
    public class LinearTranslator : Translator
    {
        // protected override Vector3 UpdatePosition(Vector3 from, Vector3 to, float delta) =>
        //     Vector3.Lerp(from, to, delta);
        public LinearTranslator(ITranslatable translatable) : base(translatable)
        {
        }
    }
}