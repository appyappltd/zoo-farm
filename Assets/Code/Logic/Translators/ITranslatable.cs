using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslatable
    {
        Vector3 Position { get; }
        void Warp(Vector3 to);
    }
}