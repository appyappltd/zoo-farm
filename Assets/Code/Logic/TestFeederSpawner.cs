using Infrastructure.Factory;
using Logic.Animals.AnimalFeeders;
using Logic.Foods.FoodSettings;
using NaughtyAttributes;
using Services;
using Services.Feeders;
using UnityEngine;

namespace Logic
{
    public class TestFeederSpawner : MonoBehaviour
    {
        [SerializeField] private FoodId _foodId;

        private IAnimalFeederService _feederService;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _feederService = AllServices.Container.Single<IAnimalFeederService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }

        [Button]
        private void SpawnFeeder()
        {
            var feeder = _gameFactory.CreateFeeder(transform.position, transform.rotation, _foodId);
            _feederService.RegisterFeeder(feeder.GetComponent<AnimalFeederView>().Feeder);
        }
    }
}