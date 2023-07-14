using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Logic.Player;
using Logic.Spawners;
using Logic.Translators;
using Services;
using Services.Animals;
using Tutorial.StaticTriggers;
using Ui.Services;
using UnityEngine;

namespace Logic.Animals
{
    [RequireComponent(typeof(RunTranslator))]
    public class AnimalReleaser : MonoBehaviour
    {
        //TODO: В дальнейшем за каждое животное разное количество денег
        [SerializeField] private int _coinsToSpawn;
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private TutorialTriggerStatic _animalReleasedTrigger;

        private CollectibleCoinSpawner _spawner;
        private IAnimalsService _animalService;
        private IWindowService _windowService;

        private void Awake()
        {
            _spawner = GetComponent<CollectibleCoinSpawner>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _windowService = AllServices.Container.Single<IWindowService>();
            _animalService.Released += OnReleased;
            _playerInteraction.Interacted += OnCompleteDelay;
        }

        private void OnCompleteDelay(Hero _)
        {
            _windowService.Open(WindowId.AnimalRelease);
        }

        private void OnDestroy()
        {
            _animalService.Released -= OnReleased;
            _playerInteraction.Interacted -= OnCompleteDelay;
        }

        private void OnReleased(AnimalId type)
        {
            Debug.Log("Spawn coins");
            _animalReleasedTrigger.Trigger(gameObject);
            _spawner.Spawn(_coinsToSpawn);
        }
    }
}
