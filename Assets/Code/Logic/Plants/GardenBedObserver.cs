using System;
using Data.ItemsData;
using Logic.Spawners;
using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Plants
{
    public class GardenBedOperator : MonoBehaviour, IGetItem
    {
        [SerializeField] private GardenBed _gardenBed;
        [SerializeField] private HandItemSpawner _handItemSpawner;
        
        private HandItem _spawnedHandItem;
        private bool _isHandItemReady;
        
        public event Action<IItem> Removed = i => { };
        
        private void Awake()
        {
            _gardenBed.GrowUp += OnGrowUp;
        }

        private void OnDestroy()
        {
            _gardenBed.GrowUp -= OnGrowUp;
        }

        private void OnGrowUp()
        {
            _spawnedHandItem = _handItemSpawner.Spawn();
            _isHandItemReady = true;
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
    }
}