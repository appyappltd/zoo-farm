using System.Collections.Generic;
using Data.ItemsData;
using DelayRoutines;
using Logic.Storages;
using Logic.Storages.Items;
using NaughtyAttributes;
using Observables;
using Progress;
using UnityEngine;

namespace Logic.Animals
{
    public class Bowl : MonoBehaviour, IProgressBarProvider
    {
        private const float ReplenishDelayAfterEmptyFood = 0.2f;
#if UNITY_EDITOR
        [ProgressBar("Food", 4f, EColor.Green)]
        [SerializeField] private float _foodValue;
#endif

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly List<RoutineSequence> _routines = new List<RoutineSequence>();

        [SerializeField] private Transform _bowlPlace;
        [SerializeField] private Transform _eatPlace;

        private IInventory _inventory;
        private ProgressBar _food;
        private IItem _eatenFood;

        private bool _isReplenishing;
        private int _foodLeft;
        private int _maxFoodAmount;
        
        public IProgressBar ProgressBarView => _food;
        public Transform EatPlace => _eatPlace;

        private void OnDestroy()
        {
            if (_inventory != null)
            {
                _inventory.Added -= ReplenishFromInventory;
                _food.Empty -= OnEmptyFood;
                _food.Full -= OnFullFood;
                _compositeDisposable.Dispose();
            }
        }

        public void Construct(IInventory inventory)
        {
            _maxFoodAmount = _inventory.MaxWeight; 
            _inventory = inventory;
            _food = new ProgressBar(_inventory.MaxWeight);
            Subscribe();
            _isReplenishing = true;
        }

        public void Construct(IInventory inventory, int maxFoodAmount)
        {
            _maxFoodAmount = maxFoodAmount;
            _inventory = inventory;
            _food = new ProgressBar(maxFoodAmount);
            Subscribe();
            _isReplenishing = true;
        }
        
        private void Subscribe()
        {
            _inventory.Added += ReplenishFromInventory;
            _food.Empty += OnEmptyFood;
            _food.Full += OnFullFood;
            _compositeDisposable.Add(_food.Current.Then(OnSpend));
#if UNITY_EDITOR
            _compositeDisposable.Add(_food.Current.Then((f => _foodValue = f)));
#endif
        }

        private void ReplenishFromInventory(IItem item) =>
            Replenish(item.Weight);

        private void Replenish(int amount)
        {
            if (_isReplenishing)
            {
                _food.Replenish(amount);
            }
            else
            {
                RoutineSequence routineSequence = new RoutineSequence()
                    .WaitUntil(() => _food.IsEmpty)
                    .WaitForSeconds(ReplenishDelayAfterEmptyFood)
                    .Then(() => _food.Replenish(amount));
                
                _routines.Add(routineSequence);
                routineSequence.Play();
            }
        }

        private void OnSpend()
        {
            if (Mathf.FloorToInt(_food.Current.Value) >= _foodLeft)
                return;

            ClearEatenFood();

            if (_inventory.TryGet(ItemId.Food, out IItem item))
            {
                _foodLeft--;
                item.Mover.Move(EatPlace, EatPlace);
                _eatenFood = item;
            }
        }

        private void OnEmptyFood()
        {
            _isReplenishing = true;
            ClearEatenFood();
        }

        private void ClearEatenFood()
        {
            _eatenFood?.Destroy();
            _eatenFood = null;
        }

        private void OnFullFood()
        {
            _isReplenishing = false;
            _foodLeft = _maxFoodAmount;
        }

        public void Clear()
        {
            _isReplenishing = true;
            
            foreach (var routine in _routines)
                routine.Kill();
            
            _routines.Clear();

            _food.Reset();
            ClearEatenFood();
        }
    }
}