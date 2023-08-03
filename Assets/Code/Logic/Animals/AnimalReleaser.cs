using Logic.Animals.AnimalsBehaviour;
using Logic.Gates;
using Logic.Interactions;
using Logic.Player;
using Logic.Spawners;
using Logic.Translators;
using Services;
using Services.Animals;
using StaticData;
using Tutorial.StaticTriggers;
using Ui.Services;
using Unity.VisualScripting;
using UnityEngine;

namespace Logic.Animals
{
    [RequireComponent(typeof(RunTranslator))]
    public class AnimalReleaser : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private AnimalInteraction _animalInteraction;
        [SerializeField] private TutorialTriggerStatic _animalReleasedTrigger;
        [SerializeField] private CollectibleCoinSpawner _spawner;
        [SerializeField] private Transform _releaseOutPlace;
        [SerializeField] private Gate _gate;

        [Space] [Header("Settings")]
        [SerializeField] private ReleaseCoinsConfig _releaseCoinsConfig;

        private IAnimalsService _animalService;
        private IWindowService _windowService;
        private int _releasingAnimalCount;

        private void Awake()
        {
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _windowService = AllServices.Container.Single<IWindowService>();

            _animalService.Released += OnReleased;
            _playerInteraction.Interacted += OnInteracted;
            _animalInteraction.Interacted += OnGatePassed;
        }

        private void OnDestroy()
        {
            _animalService.Released -= OnReleased;
            _playerInteraction.Interacted -= OnInteracted;
            _animalInteraction.Interacted -= OnGatePassed;
        }

        private void OnGatePassed(IAnimal animal)
        {
            _releasingAnimalCount--;
            _spawner.Spawn(CalculateCoins(animal));

            if (_releasingAnimalCount <= 0)
                _gate.Close();
        }

        private int CalculateCoins(IAnimal animal) =>
            _releaseCoinsConfig.Coins(animal.AnimalId.Type) * animal.HappinessFactor.Factor;

        private void OnInteracted(Hero _) =>
            _windowService.Open(WindowId.AnimalRelease);

        private void OnReleased(IAnimal animal)
        {
            _releasingAnimalCount++;
            animal.ForceMove(_releaseOutPlace);
            _animalReleasedTrigger.Trigger(gameObject);
            _gate.Open();
        }
    }
}