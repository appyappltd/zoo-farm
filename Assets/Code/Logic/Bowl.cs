using Data.ItemsData;
using Logic.Storages;
using Logic.Storages.Items;
using Observables;
using Progress;
using UnityEngine;

namespace Logic
{
    public class Bowl : MonoBehaviour, IProgressBarProvider
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private IInventory _inventory;
        private ProgressBar _food;
        private IItem _eatenFood;

        public IProgressBar ProgressBarView => _food;

        private void OnDestroy()
        {
            _inventory.Added -= ReplenishFromInventory;
            _compositeDisposable.Dispose();
        }

        public void Construct(IInventory inventory)
        {
            _inventory = inventory;
            _food = new ProgressBar(_inventory.MaxWeight, 0);

            _inventory.Added += ReplenishFromInventory;
            _food.Empty += OnEmptyFood;
            _compositeDisposable.Add(_food.Current.Then(OnSpend));
        }

        private void ReplenishFromInventory(IItem item)
        {
            Replenish(item.Weight);
        }

        private void Replenish(int amount)
        {
            _food.Replenish(amount);
        }

        private void OnSpend()
        {
            if (Mathf.RoundToInt(_food.Current.Value) >= _inventory.Weight)
                return;

            _eatenFood?.Destroy();

            if (_inventory.TryGet(ItemId.Food, out IItem item))
            {
                item.Mover.Move(transform, transform);
                _eatenFood = item;
            }
        }

        private void OnEmptyFood()
        {
            _eatenFood.Destroy();
            _eatenFood = null;
        }
    }
}