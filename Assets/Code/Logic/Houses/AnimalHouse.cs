using System;
using Data.ItemsData;
using Logic.Animals;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using Logic.Storages.Items;
using NTC.Global.System;
using Ui;
using UnityEngine;

namespace Logic.Houses
{
    public class AnimalHouse : MonoBehaviour, IAnimalHouse
    {
        private const string ThisHouseIsAlreadyTaken = "This house is already taken";
        private const string ThisHouseIsAlreadyFree = "This house is already free";

        [Space] [Header("References")]
        [SerializeField] private Bowl _bowl;
        [SerializeField] private Storage _storage;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private BarIconView _barIconView;

        [Header("Settings")]
        [SerializeField] private Transform _restPlace;
        [SerializeField] private Transform _eatPlace;
        [SerializeField] private AnimalType _forAnimal;
        [SerializeField] private bool _isTaken;

        private AnimalId _animalId;

        public event Action<IAnimalHouse> BowlEmpty = _ => { };

        public AnimalId AnimalId => _animalId;
        public Transform FeedingPlace => _restPlace;
        public Transform EatPlace => _eatPlace;
        public bool IsTaken => _isTaken;
        public AnimalType ForAnimal => _forAnimal;
        public FoodId EdibleFoodType => _animalId.EdibleFood;
        public IInventory Inventory => _inventoryHolder.Inventory;

        private void Awake() =>
            Init();

        private void OnDestroy() =>
            _bowl.ProgressBarView.Empty -= OnBowlEmpty;

        public void AttachAnimal(AnimalId animalId)
        {
            if (_isTaken)
                throw new Exception(ThisHouseIsAlreadyTaken);

            _animalId = animalId;
            _isTaken = true;
        }

        public void DetachAnimal()
        {
            if (_isTaken == false)
                throw new Exception(ThisHouseIsAlreadyFree);

            _isTaken = false;
            _animalId = null;
        }

        public void Clear()
        {
            _bowl.Clear();
            _barIconView.Enable();

            while (_inventoryHolder.Inventory.TryGet(ItemId.All, out IItem result))
                result.Destroy();
        }

        private void Init()
        {
            _inventoryHolder.Construct();
            _bowl.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);
            _barIconView.Construct(_bowl.ProgressBarView);
            _productReceiver.Construct(_inventoryHolder.Inventory);
            _bowl.ProgressBarView.Empty += OnBowlEmpty;
        }

        private void OnBowlEmpty() =>
            BowlEmpty.Invoke(this);
    }
}