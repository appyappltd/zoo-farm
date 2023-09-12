using System.Diagnostics;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using NaughtyAttributes;
using Services;
using Services.StaticData;
using Ui;
using UnityEngine;

namespace Logic.Animals.AnimalFeeders
{
    public class AnimalFeederView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Storage _storage;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private ProductReceiver _productReceiver;
        [SerializeField] private Bowl[] _bowls;
        
        [Space] [Header("Settings")]
        [SerializeField] private FoodId _foodId;
        [SerializeField] private int _maxFoodPerBowl;

        private IStaticDataService _staticData;
        
        private AnimalFeeder _animalFeeder;

        public AnimalFeeder Feeder => _animalFeeder;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IStaticDataService>());
            Init();
        }

        private void Construct(IStaticDataService staticData) =>
            _staticData = staticData;

        private void Init()
        {
            _inventoryHolder.Construct(_maxFoodPerBowl * _bowls.Length + 1);
            _storage.Construct(_inventoryHolder.Inventory);
            _productReceiver.Construct(_inventoryHolder.Inventory);
            ConstructBowls();
            _animalFeeder = new AnimalFeeder(_bowls, _inventoryHolder.Inventory, _foodId);
        }

        private void ConstructBowls()
        {
            for (var index = 0; index < _bowls.Length; index++)
            {
                Bowl bowl = _bowls[index];
                bowl.Construct(_inventoryHolder.Inventory, _maxFoodPerBowl);

                BarIconView barIcon = bowl.GetComponentInChildren<BarIconView>();
                barIcon.Construct(bowl.ProgressBarView, _staticData.IconByFoodType(_foodId));
            }
        }

        [Button] [Conditional("UNITY_EDITOR")]
        private void CollectAllBowls() =>
            _bowls = GetComponentsInChildren<Bowl>();
    }
}