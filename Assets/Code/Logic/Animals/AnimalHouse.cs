using System;
using Logic.Storages;
using Unity.VisualScripting;
using UnityEngine;

namespace Logic.Animals
{
    public class AnimalHouse : MonoBehaviour
    {
        private const string ThisHouseIsAlreadyTaken = "This house is already taken";
        private const string ThisHouseIsAlreadyFree = "This house is already free";

        [Space] [Header("References")]
        [SerializeField] private Bowl _bowl;

        [SerializeField] private Storage _storage;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private ProductReceiver _productReceiver;

        [Header("Settings")]
        [SerializeField] private Transform _restPlace;
        [SerializeField] private Transform _eatPlace;
        [SerializeField] private AnimalType _forAnimal;
        [SerializeField] private bool _isTaken;

        private AnimalId _animalId;

        public event Action<AnimalHouse> BowlEmpty = _ => { };

        public AnimalId AnimalId => _animalId;
        public Transform RestPlace => _restPlace;
        public Transform EatPlace => _eatPlace;
        public bool IsTaken => _isTaken;
        public AnimalType ForAnimal => _forAnimal;

        private void Awake()
        {
            Construct();
        }

        private void OnDestroy() =>
            _bowl.ProgressBarView.Empty += OnBowlEmpty;

        public void Construct()
        {
            _inventoryHolder.Construct();
            _bowl.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);
            _productReceiver.Construct(_inventoryHolder.Inventory);
        }

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

        private void OnBowlEmpty() =>
            BowlEmpty.Invoke(this);
    }
}