using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Logic.Medical
{
    public class MedicalToolShower : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particles;
        [SerializeField] private SerializedDictionary<TreatToolId, Material> _materials;

        private ParticleSystemRenderer _particleSystemRenderer;

        private void Awake()
        {
            _particleSystemRenderer = _particles.GetComponent<ParticleSystemRenderer>();
        }

        public void SetTool(TreatToolId toolId)
        {
            _particleSystemRenderer.sharedMaterial = _materials[toolId];
            _particles.Play();
        }
    }
}