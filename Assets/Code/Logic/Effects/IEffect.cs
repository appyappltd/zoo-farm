using UnityEngine;

namespace Logic.Effects
{
    public interface IEffect
    {
        ParticleSystem ParticleSystem { get; }
        void Play(Vector3 at, Quaternion rotation);
        void Stop();
    }
}