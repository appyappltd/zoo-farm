using System.Collections.Generic;
using System.Diagnostics;
using AYellowpaper;
using Data.ItemsData;
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
using Services.Animals;
using Services.Breeding;
using Services.PersistentProgress;
using Services.StaticData;
using Tutorial.StaticTriggers;
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
        [SerializeField] private Transform _heartPosition;
        [SerializeField] private InterfaceReference<ITranslator, MonoBehaviour> _translator;
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _interactionsGrid;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private Storage _storage;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private HumanInteraction _interactionZone;
        [SerializeField] private Transform _breedingPlace;
        [SerializeField] private TutorialTriggerScriptableObject _breedingCompleteTrigger;

        [Space] [Header("Settings")]
        [SerializeField] [MinMaxSlider(0.1f, 30f)] private Vector2 _currencySpawnDelay;
        
        private IAnimalBreedService _breedService;
        private IStaticDataService _staticData;
        private IPersistentProgressService _persistentProgress;
        private IGameFactory _gameFactory;
        
        private BreedingCurrencySpawner _currencySpawner;
        private IItem _takenHeart;
        private IHuman _cashedHuman;
        private bool _isInProgress;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IAnimalBreedService>(),
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IPersistentProgressService>(),
                AllServices.Container.Single<IGameFactory>(),
                AllServices.Container.Single<IAnimalsService>());
            CreateAllInteractions();
        }

        private void OnDestroy()
        {
            _inventoryHolder.Inventory.Removed -= OnRemovedItem;
            _interactionZone.Canceled -= OnCancelZone;
            _interactionZone.Interacted -= OnInteractedZone;
            _disposable.Dispose();
            _currencySpawner.Dispose();
        }

        private void Construct(IAnimalBreedService breedService, IStaticDataService staticData,
            IPersistentProgressService persistentProgress, IGameFactory gameFactory, IAnimalsService animalsService)
        {
            _gameFactory = gameFactory;
            _persistentProgress = persistentProgress;
            _staticData = staticData;
            _breedService = breedService;
            
            _inventoryHolder.Construct();
            _storage.Construct(_inventoryHolder.Inventory);
            _productReceiver.Construct(_inventoryHolder.Inventory);
            
            _inventoryHolder.Inventory.Removed += OnRemovedItem;
            _interactionZone.Canceled += OnCancelZone;
            _interactionZone.Interacted += OnInteractedZone;

            _currencySpawner = new BreedingCurrencySpawner(gameFactory.HandItemFactory, animalsService.AnimalCounter, _storage, _inventoryHolder.Inventory,
                _currencySpawnDelay, _translator.Value);
        }

        private void Subscribe(ChoseInteractionProvider choseZone, AnimalType associatedType)
        {
            void OnChosen(IHuman human)
            {
                if (_isInProgress)
                    return;

                if (_breedService.TryBreeding(associatedType, out AnimalPair pair))
                {
                    ItemFilter itemFilter = new ItemFilter(ItemId.BreedingCurrency);

                    if (human.Inventory.TryGet(itemFilter, out IItem item))
                    {
                        _isInProgress = true;
                        _interactionZone.Deactivate();
                        _breedService.BeginBreeding(pair, _breedingPlace, OnBreedingBegins, OnBreedingComplete);
                        item.Mover.Move(_heartPosition, _heartPosition, true);
                    }
                    
                    _interactionsGrid.Value.RemoveAll();
                    return;
                }

#if DEBUG
                Debug.LogWarning($"Animals of {associatedType} type are not enough for reproduction");
#endif
            }

            choseZone.Interaction.Interacted += OnChosen;
            _disposable.Add(new EventDisposer(() => choseZone.Interaction.Interacted -= OnChosen));
        }

        private void OnInteractedZone(Human human) =>
            _cashedHuman = human;

        private void OnCancelZone()
        {
            ItemFilter itemFilter = new ItemFilter(ItemId.BreedingCurrency);

            if (_cashedHuman is null)
                return;

            if (_cashedHuman.Inventory.TryGet(itemFilter, out IItem item))
                _inventoryHolder.Inventory.TryAdd(item);
            
            _interactionsGrid.Value.RemoveAll();
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

        private void OnBreedingBegins()
        {
            if (_takenHeart is not ITranslatableAnimated animated)
                return;

            ITranslatableParametric<Vector3> animatedScaleTranslatable = animated.ScaleTranslatable;
            animatedScaleTranslatable.Play(Vector3.one, Vector3.zero);
            _translator.Value.Add(animatedScaleTranslatable);
            _breedingCompleteTrigger.Trigger();

            void OnEndTranslate(ITranslatable _)
            {
                animatedScaleTranslatable.End -= OnEndTranslate;
                _currencySpawner.ReturnItem(_takenHeart);
            }

            animatedScaleTranslatable.End += OnEndTranslate;
        }

        private void OnBreedingComplete()
        {
            _isInProgress = false;
            _interactionZone.Activate();
        }

        private void OnRemovedItem(IItem item)
        {
            if (_isInProgress)
                return;

            _takenHeart = item;
            
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