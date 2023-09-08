using Infrastructure.Factory;
using Logic.Animals.AnimalsBehaviour;
using Logic.Gates;
using Logic.Interactions;
using Logic.LevelGoals;
using Logic.Spawners;
using Logic.Translators;
using Services;
using Services.Animals;
using Services.PersistentProgress;
using Services.StaticData;
using StaticData;
using Tutorial.StaticTriggers;
using Ui.Services;
using UnityEngine;

namespace Logic.Animals
{
    [RequireComponent(typeof(RunTranslator))]
    public class AnimalReleaser : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private LevelGoalView _levelGoal;
        [SerializeField] private AnimalInteraction _animalInteraction;
        [SerializeField] private TutorialTriggerScriptableObject _animalReleasedTrigger;
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
            Construct(
                AllServices.Container.Single<IGameFactory>(),
                AllServices.Container.Single<IAnimalsService>(),
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IPersistentProgressService>());
        }

        private void OnDestroy()
        {
            _animalInteraction.Interacted -= OnGatePassed;
            _levelGoal.ReleaseInteraction -= OnReleaseInteracted;
        }
        
        public void Construct(IGameFactory gameFactory, IAnimalsService animalsService, IStaticDataService staticData,
            IPersistentProgressService persistenceProgress)
        {
            _animalService = animalsService;

            GoalConfig goalConfig = staticData.GoalConfigForLevel(persistenceProgress.Progress.LevelData.LevelKey);
            
            _levelGoal.Construct(gameFactory, animalsService, goalConfig);
            
            _animalInteraction.Interacted += OnGatePassed;
            _levelGoal.ReleaseInteraction += OnReleaseInteracted;
        }

        private void OnReleaseInteracted(AnimalType releaseType)
        {
            IAnimal animal = _animalService.GetReleaseReadySingle(releaseType);
            _animalService.Release(animal);
            _releasingAnimalCount++;
            animal.ForceMove(_releaseOutPlace);
            _animalReleasedTrigger.Trigger(gameObject);
            _gate.Open();
        }

        private void OnGatePassed(IAnimal animal)
        {
            _releasingAnimalCount--; 
            _spawner.Spawn(CalculateCoins(animal));

            if (_releasingAnimalCount <= 0)
                _gate.Close();
        }

        private int CalculateCoins(IAnimal animal)
        {
            Debug.Log($"release coins: {_releaseCoinsConfig.Coins(animal.AnimalId.Type) * animal.HappinessFactor.Factor}");
            return _releaseCoinsConfig.Coins(animal.AnimalId.Type) * animal.HappinessFactor.Factor;
        }
    }
}