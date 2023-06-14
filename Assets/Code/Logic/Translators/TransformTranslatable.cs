using UnityEngine;

namespace Logic.Translators
{
    public class TransformTranslatable : MonoBehaviour, ITranslatable
    {
        public Vector3 Position => transform.position;

        public void Warp(Vector3 to) =>
            transform.position = to;
    }
}