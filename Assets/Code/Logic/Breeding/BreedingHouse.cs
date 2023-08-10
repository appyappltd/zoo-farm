using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Logic.Player;
using Logic.Storages;
using Observables;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using StateMachineBase.States;
using Ui;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingHouse : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        private const int MaxAnimals = 2;

        [Space] [Header("References")]
        [SerializeField] private Bowl _bowl;
        [SerializeField] private Storage _storage;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private BarIconView _barIconView;

        [SerializeField] private Transform _firstPlace;
        [SerializeField] private Transform _secondPlace;
        [SerializeField] private Transform _childPlace;

        [SerializeField] private HumanInteraction _humanInteraction;

        private IWindowService _windowService;
        private IAnimalsService _animalService;
        private IAnimalHouseService _houseService;

        private List<IAnimal> _animals = new List<IAnimal>(2);

        private int _animalsInHouse;
        private AnimalType _breedingAnimalType;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            _humanInteraction.Interacted += OnInteracted;
            
            Init();
        }

        private void OnDestroy()
        {
            _humanInteraction.Interacted -= OnInteracted;
            _bowl.ProgressBarView.Full -= BeginEat;
        }

        private void Init()
        {
            _inventoryHolder.Construct();
            _bowl.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);
            _barIconView.Construct(_bowl.ProgressBarView);
            _productReceiver.Construct(_inventoryHolder.Inventory);

            _bowl.ProgressBarView.Full += BeginEat;
            _bowl.ProgressBarView.Empty += EndEat;
        }

        private void EndEat()
        {
            foreach (var animal in _animals)
            {
                
            }
        }

        private void BeginEat()
        {
            for (var index = 0; index < _animals.Count; index++)
            {
                IAnimal animal = _animals[index];
                animal.StateMachine.ForceEat();
            }
        }

        private void OnInteracted(Human human)
        {
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
        }

        private void MoveToPlace(IAnimal animal, Transform place)
        {
            _animals.Add(animal);
            animal.StateMachine.ForceMove(place);
            animal.Stats.Deactivate();
            animal.StateMachine.SetForceBowl(_bowl);
            _disposable.Add(animal.StateMachine.CurrentStateType.Then(state =>
            {
                if (state == typeof(Idle))
                {
                    CheckForBeginBreeding();
                }
            }));

            _houseService.VacateHouse(animal.AnimalId);
        }

        private void CheckForBeginBreeding()
        {
            _animalsInHouse++;
            
            if (_animalsInHouse >= MaxAnimals)
                BeginBreeding();
        }

        private void BeginBreeding()
        {
            Debug.Log("Begin breeding");
            _gameFactory.CreateAnimalChild(_childPlace.position, _childPlace.rotation, _breedingAnimalType);
        }
    }
}   