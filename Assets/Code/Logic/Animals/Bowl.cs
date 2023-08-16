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

        [SerializeField] private Transform _bowlPlace;

        private IInventory _inventory;
        private ProgressBar _food;
        private IItem _eatenFood;

        private bool _isReplenishing;
        private int _foodLeft;

        private List<RoutineSequence> _routines = new List<RoutineSequence>();

        public IProgressBar ProgressBarView => _food;

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
            _inventory = inventory;
            _food = new ProgressBar(_inventory.MaxWeight, 0);

            _inventory.Added += ReplenishFromInventory;
            _food.Empty += OnEmptyFood;
            _food.Full += OnFullFood;
            _compositeDisposable.Add(_food.Current.Then(OnSpend));
#if UNITY_EDITOR
            _compositeDisposable.Add(_food.Current.Then((f => _foodValue = f)));
#endif

            _isReplenishing = true;
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
                item.Mover.Move(_bowlPlace, _bowlPlace);
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
            _foodLeft = _inventory.Weight;
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