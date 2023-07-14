using Tutorial.StaticTriggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Medicine
{
    public class MedicineBedTutorialObserver : MonoBehaviour
    {
        [FormerlySerializedAs("_medicineBed")] [SerializeField] private MedicalBed _medicalBed;
        [SerializeField] private TutorialTriggerStatic _animalHealed;
        
        private void OnEnable()
        {
            _medicalBed.Healed += OnHealed;
        }

        private void OnDisable()
        {
            _medicalBed.Healed -= OnHealed;
        }

        private void OnHealed()
        {
            _animalHealed.Trigger(gameObject);
        }
    }
}