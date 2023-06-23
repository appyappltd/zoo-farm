using Data.ItemsData;
using Observables;
using Progress;
using UnityEngine;

namespace Logic
{
    public class Bowl : MonoBehaviour, IProgressBarHolder
    {
        [SerializeField] private Inventory.Inventory _inventory;

        private CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private ProgressBar _food;

        public IProgressBar ProgressBarView => _food;

        private void Awake()
        {
            _food = new ProgressBar(_inventory.MaxWeight, 0);
        }

        private void OnEnable()
        {
            _inventory.AddItem += ReplenishFromInventory;
            _compositeDisposable.Add(_food.Current.Then(OnSpend));
        }

        private void OnDisable()
        {
            _inventory.AddItem -= ReplenishFromInventory;
            _compositeDisposable.Dispose();
        }

        private void ReplenishFromInventory(HandItem item)
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
                HandItem item = _inventory.Remove();
                Destroy(item.gameObject);
            }
        }
    }
}