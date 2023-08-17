using System;
using Services.Pools;
using UnityEngine;

namespace Logic.Effects
{
    public class Effect : MonoBehaviour, ISelfPoolable, IEffect
    {
        [SerializeField] private ParticleSystem _particleSystem;

        public GameObject GameObject => gameObject;
        public ParticleSystem ParticleSystem => _particleSystem;

        public event Action<ISelfPoolable> Disabled = _ => { };

        public void Play(Vector3 at, Quaternion rotation)
        {
            transform.SetPositionAndRotation(at, rotation);
            _particleSystem.Play();
        }

        public void Play(Location location)
        {
            transform.SetPositionAndRotation(location.Position, location.Rotation);
            _particleSystem.Play();
        }

        public void Stop() =>
            Disabled.Invoke(this);

        private void OnParticleSystemStopped() =>
            Disabled.Invoke(this);
    }
}