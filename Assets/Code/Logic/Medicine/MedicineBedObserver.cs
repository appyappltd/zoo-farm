using Tutorial.StaticTriggers;
using UnityEngine;

namespace Logic.Medicine
{
    public class MedicineBedTutorialObserver : MonoBehaviour
    {
        [SerializeField] private MedicineBed _medicineBed;
        [SerializeField] private TutorialTriggerStatic _animalHealed;
        
        private void OnEnable()
        {
            _medicineBed.Healed += OnHealed;
        }

        private void OnDisable()
        {
            _medicineBed.Healed -= OnHealed;
        }

        private void OnHealed()
        {
            _animalHealed.Trigger(gameObject);
        }
    }
}