using Logic.Animals;
using Infrastructure.Factory;
using NaughtyAttributes;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic
{
    public class TestHouseSpawner : MonoBehaviour
    {
        [SerializeField] private AnimalType _type;
        
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }
        
        [Button]
        private void SpawnHouse()
        {
            _gameFactory.CreateAnimalHouse(transform.position, transform.rotation, _type);
        }
    }
}