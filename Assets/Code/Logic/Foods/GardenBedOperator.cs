using System;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using Logic.Storages.Items;
using Services;
using UnityEngine;

namespace Logic.Foods
{
    public class GardenBedOperator : MonoBehaviour, IGetItem
    {
        [SerializeField] private GardenBed _gardenBed;
        [SerializeField] private Transform _spawnPlace;

        private HandItem _spawnedHandItem;
        private bool _isHandItemReady;
        private IHandItemFactory _handItemFactory;
        private FoodItemData _foodItemData;

        public event Action<IItem> Removed = _ => { };
        
        private void Awake()
        {
            _handItemFactory = AllServices.Container.Single<IGameFactory>().HandItemFactory;
            _gardenBed.GrowUp += OnGrowUp;
        }

        private void OnDestroy()
        {
            _gardenBed.GrowUp -= OnGrowUp;
        }

        public void Construct(FoodItemData foodItemData)
        {
            _foodItemData = foodItemData;
        }
        
        public IItem Get()
        {
            Removed.Invoke(_spawnedHandItem);
            _gardenBed.PlantNew();
            _isHandItemReady = false;
            return _spawnedHandItem;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;

            if (_isHandItemReady && byId == _spawnedHandItem.ItemId)
            {
                item = _spawnedHandItem;
                return true;
            }

            return false;
        }

        public bool TryGet(ItemId byId, out IItem result)
        {
            if (TryPeek(byId, out result))
            {
                Get();
                return true;
            }

            return false;
        }

        private void OnGrowUp()
        {
            _spawnedHandItem = _handItemFactory.CreateFood(_spawnPlace.position, _spawnPlace.rotation, _foodItemData.FoodId);
            _spawnedHandItem.Construct(_foodItemData);
            _isHandItemReady = true;
        }
    }
}