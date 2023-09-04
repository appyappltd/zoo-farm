using System;
using System.Collections.Generic;
using AYellowpaper;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Interactions;
using Logic.Player;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.TransformGrid;
using Observables;
using Services;
using Services.Breeding;
using Services.PersistentProgress;
using Services.StaticData;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingZone : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        private readonly Dictionary<AnimalType, ChoseInteractionProvider> _choseInteractions =
            new Dictionary<AnimalType, ChoseInteractionProvider>();
        
        [SerializeField] private InterfaceReference<ITransformGrid> _interactionsGrid;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private Storage _storage;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private HumanInteraction _interactionZone;
        [SerializeField] private Transform _breedingPlace;

        private IAnimalBreedService _breedService;
        
        private BreedingCurrencyContainer _currencyContainer;

        private IStaticDataService _staticData;
        private IPersistentProgressService _persistentProgress;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IAnimalBreedService>(),
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IPersistentProgressService>(),
                AllServices.Container.Single<IGameFactory>());
            CreateAllInteractions();
            
            _inventoryHolder.Construct();
            _storage.Construct(_inventoryHolder.Inventory);
            _productReceiver.Construct(_inventoryHolder.Inventory);
            
            _inventoryHolder.Inventory.Removed += OnRemovedItem;
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
        }

        private void CreateAllInteractions()
        {
            foreach (AnimalType animalType in _staticData.GoalConfigForLevel(_persistentProgress.Progress.LevelData.LevelKey).GetAnimalsToRelease())
            {
                 ChoseInteractionProvider provider = _gameFactory.CreateChoseInteraction(Vector3.zero, Quaternion.identity, animalType);
                 provider.transform.SetParent(transform);
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

#if DEBUG
                Debug.LogWarning($"Animals of {associatedType} type are not enough for reproduction");
#endif
            }

            choseZone.Interaction.Interacted += OnChosen;
            _disposable.Add(new EventDisposer(() => choseZone.Interaction.Interacted -= OnChosen));
        }
        
        private void OnRemovedItem(IItem item)
        {
            foreach (var pairType in _breedService.GetAvailablePairTypes()) 
                _interactionsGrid.Value.AddCell(_choseInteractions[pairType].transform);
        }
    }
}