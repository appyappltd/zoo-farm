using System;
using System.Collections.Generic;
using Data.ItemsData;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using Logic.Storages.Items;
using Services;
using Services.Food;
using StaticData;
using UnityEngine;

namespace Logic.Foods.Vendor
{
    public class FoodVendor : MonoBehaviour, IFoodVendorView
    {
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private FoodVendorConfig _vendorConfig;
        
        private RoutineSequence _routineSequence;
        private Stack<IItem> _readyFoods;
        private IHandItemFactory _factory;
        
        private bool _isFull;

        public event Action<IItem> Removed = _ => { };
        public event Action<IItem> FoodProduced = _ => { };
        public event Action BeginProduceFood = () => { };

        public FoodId Type => _vendorConfig.FoodId;
        public bool IsReady => _readyFoods.Count > 0;
        public Vector3 Position => transform.position;
        public int MaxFoodCount => _vendorConfig.MaxStockQuantity;
        
        private void OnDestroy() =>
            _routineSequence.Kill();

        private void Awake()
        {
            Init();
            StartProduce();
        }

        private void Init()
        {
            _factory = AllServices.Container.Single<IGameFactory>().HandItemFactory;
            AllServices.Container.Single<IFoodService>().Register(this);
            _readyFoods = new Stack<IItem>(_vendorConfig.MaxStockQuantity);

            _routineSequence = new RoutineSequence()
                .Then(OnBeginProduceFood)
                .WaitForRandomSeconds(_vendorConfig.ProduceDurationRange)
                .Then(CreateFood)
                .Then(OnFoodProduced)
                .WaitWhile(IsFull)
                .LoopWhile(() => enabled);
        }

        protected virtual void OnBeginProduceFood() =>
            BeginProduceFood.Invoke();

        protected virtual void OnFoodProduced() =>
            FoodProduced.Invoke(_readyFoods.Peek());

        IItem IGetItem.Get()
        {
            _isFull = false;
            IItem handItem = _readyFoods.Pop();
            Removed.Invoke(handItem);
            return handItem;
        }

        bool IGetItem.TryPeek(ItemFilter filter, out IItem item)
        {
            if (filter.Contains(ItemId.Food))
                return _readyFoods.TryPeek(out item);
            
            item = null;
            return false;
        }

        bool IGetItem.TryGet(ItemFilter filter, out IItem result)
        {
            if (((IGetItem) this).TryPeek(filter, out result))
            {
                result = ((IGetItem) this).Get();
                return true;
            }

            return false;
        }

        public void StartProduce() =>
            _routineSequence.Play();

        public void StopProduce() =>
            _routineSequence.Stop();

        private void CreateFood()
        {
            HandItem food = _factory.CreateFood(_spawnPlace.position, _spawnPlace.rotation, _vendorConfig.FoodId);
            food.transform.SetParent(transform, true);
            food.Construct(_vendorConfig.HandItemData);
            _readyFoods.Push(food);

            if (_readyFoods.Count == _vendorConfig.MaxStockQuantity)
                _isFull = true;
        }

        private bool IsFull() =>
            _isFull;
    }
}
