using Logic.Animals;
using Tutorial.StaticTriggers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.Medical
{
    public class MedicalBedTutorialObserver : MonoBehaviour
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

        private void OnHealed(AnimalId _)
        {
            _animalHealed.Trigger(gameObject);
        }
    }
}