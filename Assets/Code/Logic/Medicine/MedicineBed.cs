using System;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Logic.Storages;
using Logic.Storages.Items;
using NTC.Global.Cache;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.Medicine
{
    [RequireComponent(typeof(TimerOperator))]
    public class MedicineBed : MonoCache, IAddItem
    {
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private PlayerInteraction _playerInteraction;
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] [Range(1f, 5f)] private float _healingTime = 2.5f;

        private AnimalItemData _animalItem;
        private MedToolItemData _medToolItem;

        private IGameFactory _gameFactory;
        private IAnimalHouseService _houseService;

        private bool _isHealing;
        
        public event Action<IItem> Added = i => { };
        public event Action Healed = () => { };
        
        private void Awake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_healingTime, OnHealed);
            
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
        }

        protected override void OnEnabled()
        {
            _playerInteraction.Entered += OnEntered;
            _playerInteraction.Canceled += OnCanceled;
        }

        protected override void OnDisabled()
        {
            _playerInteraction.Entered -= OnEntered;
            _playerInteraction.Canceled -= OnCanceled;
        }

        public void Add(IItem item)
        {
            if (ItemIsAnimal(item))
                _animalItem = item.ItemData as AnimalItemData;

            if (ItemIsMedTool(item))
            {
                _medToolItem = item.ItemData as MedToolItemData;
                BeginHeal();
            }
            
            Added.Invoke(item);
        }

        public bool CanAdd(IItem item) =>
            CanPlaceAnimal(item) || CanPlaceMedTool(item);

        public bool TryAdd(IItem item)
        {
            if (CanAdd(item) == false)
                return false;
            
            Add(item);
            return true;
        }

        private void OnHealed()
        {
            Debug.Log("Healed");
            Healed.Invoke();
            _isHealing = false;
            
            _houseService.TakeQueueToHouse(() =>
            {
                Animal animal = _gameFactory.CreateAnimal(_animalItem.AnimalId.Type, _spawnPlace.position)
                    .GetComponent<Animal>();

                FreeTheBad();
                return animal;
            });
        }

        private void OnCanceled()
        {
            _timerOperator.Pause();
        }

        private void OnEntered(HeroProvider _)
        {
            if (_isHealing)
                _timerOperator.Play();
        }

        private void FreeTheBad()
        {
            _animalItem = null;
            _medToolItem = null;
        }

        private void BeginHeal()
        {
            _timerOperator.Restart();
            _isHealing = true;
            Debug.Log("Begin heal");
        }

        private bool CanPlaceAnimal(IItem item) =>
            ItemIsAnimal(item) && HasAnimal() == false;

        private bool CanPlaceMedTool(IItem item)
        {
            return ItemIsMedTool(item)
                   && HasAnimal()
                   && IsSuitableTool(item)
                   && HasMedTool();
        }

        private bool HasMedTool() =>
            _medToolItem is null;

        private bool IsSuitableTool(IItem item) =>
            _animalItem.TreatTool == (item as MedToolItemData).MedicineTool;

        private bool HasAnimal() =>
            _animalItem is not null;

        private bool ItemIsMedTool(IItem item) =>
            (item.ItemId & ItemId.Medical) != 0;

        private bool ItemIsAnimal(IItem item) =>
            (item.ItemId & ItemId.Animal) != 0;
    }
}
