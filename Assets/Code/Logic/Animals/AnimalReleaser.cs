using Logic.Animals.AnimalsBehaviour;
using Logic.Gates;
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
        [Header("References")]
        // [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private AnimalInteraction _animalInteraction;
        [SerializeField] private TutorialTriggerStatic _animalReleasedTrigger;
        [SerializeField] private CollectibleCoinSpawner _spawner;
        [SerializeField] private Transform _releaseOutPlace;
        [SerializeField] private Gate _gate;

        [Space] [Header("Settings")] [SerializeField]
        private int _coinsToSpawn;

        // [SerializeField] [Range(0f, 3f)] private float _coinsSpawnDelay = 1f;

        private IAnimalsService _animalService;
        private IWindowService _windowService;
        private int _releasingAnimalCount;

        private void Awake()
        {
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _windowService = AllServices.Container.Single<IWindowService>();
            _animalService.Released += OnReleased;
            // _timerOperator.SetUp(_coinsSpawnDelay, () => _spawner.Spawn(_coinsToSpawn));
            _playerInteraction.Interacted += OnInteracted;
            _animalInteraction.Interacted += OnGatePassed;
        }

        private void OnGatePassed(IAnimal animal)
        {
            _releasingAnimalCount--;
            _spawner.Spawn(_coinsToSpawn);

            if (_releasingAnimalCount <= 0)
                _gate.Close();
        }

        private void OnInteracted(Hero _) =>
            _windowService.Open(WindowId.AnimalRelease);

        private void OnDestroy()
        {
            _animalService.Released -= OnReleased;
            _playerInteraction.Interacted -= OnInteracted;
        }

        private void OnReleased(IAnimal animal)
        {
            _releasingAnimalCount++;
            animal.ForceMove(_releaseOutPlace);
            _animalReleasedTrigger.Trigger(gameObject);
            _gate.Open();
        }
    }
}