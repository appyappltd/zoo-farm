using Logic.Storages;
using Logic.Storages.Items;
using Observables;
using Progress;
using UnityEngine;

namespace Logic
{
    public class Bowl : MonoBehaviour, IProgressBarHolder, IInventoryProvider
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        
        [SerializeField] private int _maxInventoryWeight;

        private IInventory _inventory;
        private ProgressBar _food;

        public IProgressBar ProgressBarView => _food;

        public IGetItemObserver GetItemObserver => _inventory;
        public IAddItemObserver AddItemObserver => _inventory;

        private void Awake()
        {
            _inventory = new Inventory(_maxInventoryWeight);
            _food = new ProgressBar(_inventory.MaxWeight, 0);
        }

        private void OnEnable()
        {
            _inventory.Added += ReplenishFromInventory;
            _compositeDisposable.Add(_food.Current.Then(OnSpend));
        }

        private void OnDisable()
        {
            _inventory.Added -= ReplenishFromInventory;
            _compositeDisposable.Dispose();
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
            if (Mathf.RoundToInt(_food.Current.Value) < _inventory.Weight)
            {
                IItem item = _inventory.Get();
                item.Destroy();
            }
        }

    }
}