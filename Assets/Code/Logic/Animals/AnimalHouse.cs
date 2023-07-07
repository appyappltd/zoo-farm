using System;
using Logic.Animals.AnimalsBehaviour;
using Logic.Storages;
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
        [SerializeField] private bool _isTaken;
        
        private AnimalId _animalId;

        public AnimalId AnimalId => _animalId;
        public Transform RestPlace => _restPlace;
        public Transform EatPlace => _eatPlace;
        public bool IsTaken => _isTaken;

        private void Awake()
        {
            Construct();
        }

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
    }
}