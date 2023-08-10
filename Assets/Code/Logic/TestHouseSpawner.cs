using Infrastructure.Factory;
using Logic.Animals;
using NaughtyAttributes;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic
{
    public class TestHouseSpawner : MonoBehaviour
    {
        [SerializeField] private AnimalType _type;

        private IAnimalHouseService _houseService;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
        }
        
        [Button]
        private void SpawnHouse()
        {
            AnimalHouse house = _gameFactory.CreateAnimalHouse(transform.position, transform.rotation, _type).GetComponent<AnimalHouse>();
            _houseService.RegisterHouse(house);
        }
    }
}