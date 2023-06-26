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
                Debug.Log(animal);
            }
        }

        [Button("Release")]
        private void Release()
        {
            IAnimal animal = _animalService.Animals[_releaseIndexAnimal];
            _animalService.Release(animal);
            animal.Destroy();
        }
    }
}