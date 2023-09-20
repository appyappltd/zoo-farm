using AYellowpaper.SerializedCollections;
using UnityEngine;
using Debug = Sisus.Debugging.Debug;

namespace Code.Logic
{
    public abstract class TypedParticleMaterialSetter<T> : MonoBehaviour
    {
        [SerializeField] protected SerializedDictionary<T, Material> _materials;
        [SerializeField] private ParticleSystem _particles;
        
        private ParticleSystemRenderer _particleSystemRenderer;

        public void SetMaterial(T byType)
        {
            ApplyRenderer();

            _particleSystemRenderer.sharedMaterial = _materials[byType];
            _particles.Play();
        }

        private void ApplyRenderer() =>
            _particleSystemRenderer ??= _particles.GetComponent<ParticleSystemRenderer>();
    }
}