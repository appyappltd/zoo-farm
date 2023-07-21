using Logic.Animals;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Logic.Medical
{
    public class MedicalBedTutorialObserver : MonoBehaviour
    {
        [SerializeField] private MedicalBed _medicalBed;
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