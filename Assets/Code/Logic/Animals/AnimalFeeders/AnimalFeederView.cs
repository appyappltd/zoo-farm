using System.Diagnostics;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using NaughtyAttributes;
using Services;
using Services.Feeders;
using UnityEngine;

namespace Logic.Animals.AnimalFeeders
{
    public class AnimalFeederView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Storage _storage;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private Bowl[] _bowls;
        
        [Space] [Header("Settings")]
        [SerializeField] private FoodId _foodID;
        [SerializeField] private int _maxFoodPerBowl;

        private AnimalFeeder _animalFeeder;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IAnimalFeederService>());
        }

        private void Construct(IAnimalFeederService feederService)
        {
            ConstructBowls();
            _inventoryHolder.Construct(_maxFoodPerBowl * _bowls.Length + 1);
            _storage.Construct(_inventoryHolder.Inventory);
            _animalFeeder = new AnimalFeeder(_bowls, _foodID);
            feederService.Register(_animalFeeder);
        }

        private void ConstructBowls()
        {
            for (var index = 0; index < _bowls.Length; index++)
            {
                Bowl bowl = _bowls[index];
                bowl.Construct(_inventoryHolder.Inventory, _maxFoodPerBowl);
            }
        }

        [Button] [Conditional("UNITY_EDITOR")]
        private void CollectAllBowls() =>
            _bowls = GetComponentsInChildren<Bowl>();
    }
}