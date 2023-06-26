using Logic.Animals.AnimalsBehaviour;
using NaughtyAttributes;
using Services;
using Services.Animals;
using UnityEngine;

namespace Logic
{
    public class TestAnimalService : MonoBehaviour
    {
        [SerializeField] private int _releaseIndexAnimal;

        private IAnimalsService _animalService;

        private void Awake()
        {
            _animalService = AllServices.Container.Single<IAnimalsService>();
        }

        [Button("Show")]
        private void ShowAnimalsList()
        {
            foreach (IAnimal animal in _animalService.Animals)
            {
                Debug.Log(animal.ToString());
            }
        }

        [Button("Release")]
        private void Release()
        {
            _animalService.Release(_animalService.Animals[_releaseIndexAnimal]);
        }
    }
}