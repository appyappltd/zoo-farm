using System.Collections.Generic;
using System.Diagnostics;
using AYellowpaper;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Interactions;
using Logic.Player;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.TransformGrid;
using Logic.Translators;
using NaughtyAttributes;
using NTC.Global.System;
using Observables;
using Services;
using Services.Breeding;
using Services.PersistentProgress;
using Services.StaticData;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Logic.Breeding
{
    [RequireComponent(typeof(RunTranslator))]
    public class BreedingZone : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Dictionary<AnimalType, ChoseInteractionProvider> _choseInteractions =
            new Dictionary<AnimalType, ChoseInteractionProvider>();

        [Header("References")]
        [SerializeField] private RunTranslator _translator;
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _interactionsGrid;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private Storage _storage;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private HumanInteraction _interactionZone;
        [SerializeField] private Transform _breedingPlace;

        [Space] [Header("Settings")]
        [SerializeField] [MinMaxSlider(0.1f, 30f)] private Vector2 _currencySpawnDelay;
        
        private IAnimalBreedService _breedService;
        private IStaticDataService _staticData;
        private IPersistentProgressService _persistentProgress;
        private IGameFactory _gameFactory;
        
        private BreedingCurrencySpawner _currencySpawner;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IAnimalBreedService>(),
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IPersistentProgressService>(),
                AllServices.Container.Single<IGameFactory>());
            CreateAllInteractions();
        }

        private void OnDestroy() =>
            _disposable.Dispose();

        private void Construct(IAnimalBreedService breedService, IStaticDataService staticData,
            IPersistentProgressService persistentProgress, IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _persistentProgress = persistentProgress;
            _staticData = staticData;
            _breedService = breedService;
            
            _inventoryHolder.Construct();
            _storage.Construct(_inventoryHolder.Inventory);
            _productReceiver.Construct(_inventoryHolder.Inventory);
            
            _inventoryHolder.Inventory.Removed += OnRemovedItem;

            _currencySpawner = new BreedingCurrencySpawner(gameFactory.HandItemFactory, _storage, _inventoryHolder.Inventory,
                _currencySpawnDelay, _translator);
        }

        private void CreateAllInteractions()
        {
            foreach (AnimalType animalType in _staticData.GoalConfigForLevel(_persistentProgress.Progress.LevelData.LevelKey).GetAnimalsToRelease())
            {
                 ChoseInteractionProvider provider = _gameFactory.CreateChoseInteraction(Vector3.zero, Quaternion.identity, animalType);
                 provider.transform.SetParent(transform);
                 provider.gameObject.Disable();
                 _choseInteractions.Add(animalType, provider);
                 
                 Subscribe(provider, animalType);
            }
        }

        private void Subscribe(ChoseInteractionProvider choseZone, AnimalType associatedType)
        {
            void OnChosen(Human human)
            {
                if (_breedService.TryBreeding(associatedType, out AnimalPair pair))
                    _breedService.BeginBreeding(pair, _breedingPlace);
                
                _interactionsGrid.Value.RemoveAll();

#if DEBUG
                Debug.LogWarning($"Animals of {associatedType} type are not enough for reproduction");
#endif
            }

            choseZone.Interaction.Interacted += OnChosen;
            _disposable.Add(new EventDisposer(() => choseZone.Interaction.Interacted -= OnChosen));
        }
        
        private void OnRemovedItem(IItem _)
        {
            foreach (var pairType in _breedService.GetAvailablePairTypes()) 
                _interactionsGrid.Value.AddCell(_choseInteractions[pairType].transform);
        }
        
        [Button] [Conditional("UNITY_EDITOR")]
        private void BeginBreed()
        {
            foreach (var pairType in _breedService.GetAvailablePairTypes())
            {
                Debug.Log($"AvailableBreedType {pairType}");
                _interactionsGrid.Value.AddCell(_choseInteractions[pairType].transform);
            }
        }
    }
}