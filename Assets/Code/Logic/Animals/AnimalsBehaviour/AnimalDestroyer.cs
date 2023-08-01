using Logic.Interactions;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public class AnimalDestroyer : MonoBehaviour
    {
        [SerializeField] private AnimalInteraction _animalInteraction;

        private void Awake() =>
            _animalInteraction.Interacted += OnInteracted;

        private void OnDestroy() =>
            _animalInteraction.Interacted -= OnInteracted;

        private void OnInteracted(IAnimal animal) =>
            animal.Destroy();
    }
}