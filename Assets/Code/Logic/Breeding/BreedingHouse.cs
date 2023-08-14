using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using DelayRoutines;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Player;
using Logic.SpriteUtils;
using Logic.Storages;
using NTC.Global.System;
using Observables;
using Progress;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using Services.StaticData;
using StateMachineBase.States;
using Ui;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingHouse : MonoBehaviour
    {
        private const int MaxAnimals = 2;

        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        [Header("References")]
        [SerializeField] private Bowl _bowl;
        [SerializeField] private Storage _storage;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private BarIconView _barIconView;
        [SerializeField] private SpriteFillMask _growthBar;
        [SerializeField] private Transform _firstPlace;
        [SerializeField] private Transform _secondPlace;
        [SerializeField] private Transform _childPlace;
        [SerializeField] private HumanInteraction _humanInteraction;
        [SerializeField] private InteractionView _defaultInteractionView;
        [SerializeField] private InteractionView _backgroundInteractionView;

        [Space] [Header("Settings")]
        [SerializeField] private int _feedingCyclesToMaturity;

        private IWindowService _windowService;
        private IAnimalsService _animalService;
        private IAnimalHouseService _houseService;
        private IGameFactory _gameFactory;
        private IStaticDataService _staticData;

        private List<IAnimal> _animals = new List<IAnimal>(2);
        private DelayRoutine _afterBreedingDelay;

        private int _currentFeedingCycle;
        private int _animalsInHouse;
        private AnimalType _breedingAnimalType;
        private GameObject _childModel;

        private InteractionViewSwitcher _viewSwitcher;

        public bool IsBusy => _animals.Count > 0;

        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _staticData = AllServices.Container.Single<IStaticDataService>();

            _humanInteraction.Interacted += OnInteracted;

            Init();
        }

        private void OnDestroy()
        {
            _humanInteraction.Interacted -= OnInteracted;
            _bowl.ProgressBarView.Full -= BeginEat;
            _bowl.ProgressBarView.Empty -= EndEat;
            
            _afterBreedingDelay.Kill();
        }

        private void Init()
        {
            _inventoryHolder.Construct();
            _bowl.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);
            _barIconView.Construct(_bowl.ProgressBarView);
            _productReceiver.Construct(_inventoryHolder.Inventory);
            _growthBar.Deactivate();
            _inventoryHolder.Inventory.Deactivate();

            _viewSwitcher =
                new InteractionViewSwitcher(
                    _defaultInteractionView,
                    _backgroundInteractionView);
            _viewSwitcher.SwitchToBackground();

            _bowl.ProgressBarView.Full += BeginEat;
            _bowl.ProgressBarView.Empty += EndEat;

            _afterBreedingDelay = new DelayRoutine();
            _afterBreedingDelay
                .WaitForSeconds(1f)
                .Then(_disposable.Dispose)
                .Then(_inventoryHolder.Inventory.Activate);
        }

        private void BeginEat()
        {
            for (var index = 0; index < _animals.Count; index++)
            {
                IAnimal animal = _animals[index];
                animal.StateMachine.ForceEat();
            }
            
            _currentFeedingCycle++;
        }

        private void EndEat()
        {
            UpdateGrowthBar();

            if (_currentFeedingCycle >= _feedingCyclesToMaturity)
                FinishBreedingProcess();
        }

        private void UpdateGrowthBar() =>
            _growthBar.SetFill(_currentFeedingCycle / (float) _feedingCyclesToMaturity);

        private void BeginBreeding()
        {
            _growthBar.Activate();
            UpdateGrowthBar();
            _childModel = _gameFactory.CreateAnimalChild(_childPlace.position, _childPlace.rotation, _breedingAnimalType);
            _afterBreedingDelay.Play();
        }

        private void FinishBreedingProcess()
        {
            AnimalItemStaticData newAnimalType = _staticData.AnimalItemDataById(_animals[0].AnimalId.Type);
            Destroy(_childModel);
            _animals.Add( _gameFactory.CreateAnimal(newAnimalType, _childPlace.position, _childPlace.rotation)
                .GetComponent<IAnimal>());

            int animalsCount = _animals.Count;
            
            for (var index = 0; index < animalsCount; index++)
            {
                IAnimal animal = _animals.First();
                SendAnimalToFreeMoving(animal);
            }
            
            _inventoryHolder.Inventory.Deactivate();
            _viewSwitcher.SwitchToBackground();
        }

        private void SendAnimalToFreeMoving(IAnimal animal)
        {
            _houseService.TakeQueueToHouse(new QueueToHouse(animal.AnimalId, () =>
            {
                _animals.Remove(animal);
                animal.StateMachine.Play();
                animal.Stats.Activate();
                return animal;
            }), true);
        }

        private void OnInteracted(Human human)
        {
            if (IsBusy)
                return;
            
            if (human is Hero)
            {
                GameObject window = _windowService.Open(WindowId.Breeding);
                window.GetComponent<BreedingWindow>().SetOnChoseCallback(OnAnimalChosen);
            }
        }

        private void OnAnimalChosen(AnimalType type)
        {
            _breedingAnimalType = type;
            BreedingPair pair = _animalService.SelectPairForBreeding(type);
            MoveToPlace(pair.First, _firstPlace);
            MoveToPlace(pair.Second, _secondPlace);
            _viewSwitcher.SwitchToDefault();
        }

        private void MoveToPlace(IAnimal animal, Transform place)
        {   
            _animals.Add(animal);
            animal.StateMachine.ForceMove(place);
            animal.Stats.Deactivate();
            
            IProgressBar statsSatiety = (IProgressBar) animal.Stats.Satiety;
            statsSatiety.SetMaxNonFull();
            animal.StateMachine.SetForceBowl(_bowl);
            
            IProgressBar statsVitality = (IProgressBar) animal.Stats.Vitality;
            statsVitality.SetMaxNonFull();
            
            _disposable.Add(animal.StateMachine.CurrentStateType.Then(state =>
            {
                if (state == typeof(Idle))
                {
                    _animalsInHouse++;
                    CheckForBeginBreeding();
                }
            }));

            _houseService.VacateHouse(animal.AnimalId);
        }

        private void CheckForBeginBreeding()
        {
            if (_animalsInHouse >= MaxAnimals)
                BeginBreeding();
        }
    }
}