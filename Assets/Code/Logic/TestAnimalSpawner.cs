using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Data.ItemsData;
using Infrastructure.Factory;
using NaughtyAttributes;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using Services.Feeders;
using UnityEngine;

namespace Logic
{
    public class TestAnimalSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private AnimalItemStaticData _animalData;
        [SerializeField] private AnimalType _releaseType;

        private IGameFactory _gameFactory;
        private IAnimalHouseService _houseService;
        private IAnimalsService _animalService;
        private IAnimalFeederService _feeder;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _feeder = AllServices.Container.Single<IAnimalFeederService>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
        }

        [Button]
        private void Spawn()
        {
            var animal = _gameFactory.CreateAnimal(_animalData, _spawnPoint.position,
                _spawnPoint.rotation).GetComponent<Animal>();
            
            if (_feeder.HasFeeder(animal.AnimalId.EdibleFood))
            {
                animal.AttachFeeder(_feeder.GetFeeder(animal.AnimalId.EdibleFood));
            }
            else
            {
                void OnUpdatedFeederService()
                {
                    if (_feeder.HasFeeder(animal.AnimalId.EdibleFood))
                    {
                        _feeder.Updated -= OnUpdatedFeederService;
                        animal.AttachFeeder(_feeder.GetFeeder(animal.AnimalId.EdibleFood));
                    }
                }
                
                _feeder.Updated += OnUpdatedFeederService;
            }
        }

        [Button]
        private void Release()
        {
            _animalService.Release(_releaseType);
        }
    }
}