using Logic.Spawners;
using Logic.Translators;
using Services;
using Services.Animals;
using UnityEngine;

namespace Logic.Animals
{
    [RequireComponent(typeof(RunTranslator))]
    public class AnimalReleaser : MonoBehaviour
    {
        //TODO: В дальнейшем за каждое животное разное количество денег
        [SerializeField] private int _coinsToSpawn;

        private CollectibleCoinSpawner _spawner;
        private IAnimalsService _animalService;

        private void Awake()
        {
            _spawner = GetComponent<CollectibleCoinSpawner>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _animalService.Released += OnReleased;
        }

        private void OnDestroy()
        {
            _animalService.Released -= OnReleased;
        }

        private void OnReleased(AnimalType type)
        {
            Debug.Log("Spawn coins");
            _spawner.Spawn(_coinsToSpawn);
        }
    }
}
