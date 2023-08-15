using System;
using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using DelayRoutines;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Infrastructure.Factory;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Foods.FoodSettings;
using Logic.Interactions;
using Logic.Player;
using Logic.SpriteUtils;
using Logic.Storages;
using NaughtyAttributes;
using Observables;
using Progress;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using Services.Effects;
using Services.StaticData;
using StateMachineBase.States;
using Ui;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingHouse : MonoBehaviour, IAnimalHouse
    {
        private const int MaxAnimals = 2;
        private const float AfterBreedingDelay = 0.707f;
        private const float DispersalDelay = 1.2f;
        private const float HomelessEmotionSetDelay = 0.15f;

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
        [SerializeField] private Transform _feedingPlace;
        
        [SerializeField] private HumanInteraction _humanInteraction;
        [SerializeField] private InteractionView _defaultInteractionView;
        [SerializeField] private InteractionView _backgroundInteractionView;

        [Space] [Header("Settings")]
        [SerializeField] private int _feedingCyclesToMaturity;
        [SerializeField] private bool _isServedByKeeper;

        private IWindowService _windowService;
        private IEffectService _effectService;
        private IAnimalsService _animalService;
        private IAnimalHouseService _houseService;
        private IGameFactory _gameFactory;
        private IStaticDataService _staticData;

        private List<IAnimal> _animals = new List<IAnimal>(2);

        private DelayRoutine _afterBreeding;
        private DelayRoutine _animalsDispersal;
        private DelayRoutine _homelessEmotionSetDelay;

        private int _currentFeedingCycle;
        private int _animalsInHouse;

        private AnimalType _breedingAnimalType;
        private FoodId _edibleFoodType;

        private GameObject _childModel;
        private InteractionViewSwitcher _viewSwitcher;
        private Location _effectsLocation;

        public event Action<IAnimalHouse> BowlEmpty = _ => { };
        
        public bool IsTaken => _animals.Count > 0;
        public Transform FeedingPlace => _feedingPlace;
        public IInventory Inventory => _inventoryHolder.Inventory;
        public FoodId EdibleFoodType => _edibleFoodType;
        public bool IsServedByKeeper => _isServedByKeeper;

        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
            _effectService = AllServices.Container.Single<IEffectService>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _staticData = AllServices.Container.Single<IStaticDataService>();

            Init();
        }

        private void OnDestroy()
        {
            _humanInteraction.Interacted -= OnInteracted;
            _bowl.ProgressBarView.Full -= OnBeginEat;
            _bowl.ProgressBarView.Empty -= OnEndEat;
            
            _afterBreeding.Kill();
            _animalsDispersal.Kill();
            _homelessEmotionSetDelay.Kill();
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
            
            _effectsLocation = new Location(transform.position, Quaternion.LookRotation(Vector3.up));

            _humanInteraction.Interacted += OnInteracted;
            _bowl.ProgressBarView.Full += OnBeginEat;
            _bowl.ProgressBarView.Empty += OnEndEat;

            InitViewSwitcher();
            InitAfterBreedingRoutine();
            InitAnimalsDispersal();
            InitHomelessEmotionDelay();
        }

        private void InitHomelessEmotionDelay()
        {
            _homelessEmotionSetDelay = new DelayRoutine();
            _homelessEmotionSetDelay
                .WaitForSeconds(HomelessEmotionSetDelay)
                .Then(ShowHomelessEmotion);
        }

        private void InitAnimalsDispersal()
        {
            _animalsDispersal = new DelayRoutine();
            _animalsDispersal
                .WaitForSeconds(DispersalDelay)
                .Then(() =>
                {
                    Debug.Log("DispersalDelay");
                    SendAnimalToFreeMoving(_animals.First());
                })
                .LoopFor(MaxAnimals + 1);
        }

        private void InitAfterBreedingRoutine()
        {
            _afterBreeding = new DelayRoutine();
            _afterBreeding
                .WaitForSeconds(AfterBreedingDelay)
                .Then(_disposable.Dispose)
                .Then(_inventoryHolder.Inventory.Activate);
        }

        private void InitViewSwitcher()
        {
            _viewSwitcher =
                new InteractionViewSwitcher(
                    _defaultInteractionView,
                    _backgroundInteractionView);
            _viewSwitcher.SwitchToBackground();
        }

        private void OnBeginEat()
        {
            if (_currentFeedingCycle >= _feedingCyclesToMaturity)
                return;

            for (var index = 0; index < _animals.Count; index++)
            {
                IAnimal animal = _animals[index];
                animal.StateMachine.ForceEat();
            }
            
            _currentFeedingCycle++;
        }

        private void OnEndEat()
        {
            BowlEmpty.Invoke(this);
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
            _afterBreeding.Play();
            _effectService.SpawnEffect(EffectId.Hearts, _effectsLocation);
        }

        private void FinishBreedingProcess()
        {
            AnimalItemStaticData newAnimalType = _staticData.AnimalItemDataById(_animals[0].AnimalId.Type);
            Destroy(_childModel);
            _animals.Add( _gameFactory.CreateAnimal(newAnimalType, _childPlace.position, _childPlace.rotation)
                .GetComponent<IAnimal>());

            _animalsDispersal.Play();
            _homelessEmotionSetDelay.Play();
            _inventoryHolder.Inventory.Deactivate();
            _viewSwitcher.SwitchToBackground();
            _growthBar.Deactivate();
        }

        private void ShowHomelessEmotion()
        {
            for (int i = 0; i < _animals.Count; i++)
            {
                _animals[i].Emotions.Show(EmotionId.Homeless);
            }
        }

        private void SendAnimalToFreeMoving(IAnimal animal)
        {
            _houseService.TakeQueueToHouse(new QueueToHouse(animal, () =>
            {
                _animals.Remove(animal);
                animal.StateMachine.Play();
                animal.Stats.Activate();
                return animal;
            }), true);
        }

        private void OnInteracted(Human human)
        {
            if (IsTaken)
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
            _edibleFoodType = pair.First.AnimalId.EdibleFood;
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

        [Button]
        private void SpawnEffect()
        {
            _effectService.SpawnEffect(EffectId.Hearts, _effectsLocation);
        }
    }
}