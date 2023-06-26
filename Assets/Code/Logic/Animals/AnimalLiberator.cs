using Logic.Spawners;
using Logic.Translators;
using NaughtyAttributes;
using Services;
using Services.Animals;
using UnityEngine;

namespace Logic.Animals
{
    [RequireComponent(typeof(RunTranslator))]
    public class AnimalLiberator : MonoBehaviour
    {
        [SerializeField] private int _coinsToSpawn;

        private CollectibleCoinSpawner _spawner;
        private IAnimalsService _animalService;

        private void Awake()
        {
            _spawner = GetComponent<CollectibleCoinSpawner>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
        }

        [Button("Release")]
        private void Release()
        {
            _animalService.Release(_animalService.Animals[0]);
            _spawner.Spawn(_coinsToSpawn);
        }
    }
}
