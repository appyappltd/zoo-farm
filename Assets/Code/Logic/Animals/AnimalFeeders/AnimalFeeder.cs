using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using Logic.Storages.Items;

namespace Logic.Animals.AnimalFeeders
{
    public class AnimalFeeder : IDisposable
    {
        private readonly Queue<Bowl> _bowls;
        private readonly Queue<Bowl> _emptyBowls;
        private readonly Stack<Bowl> _reservedBowls;
        private readonly FoodId _foodId;
        private readonly IInventory _inventory;

        private List<Action> _unsubscribeActions = new List<Action>();

        public FoodId FoodId => _foodId;
        
        public AnimalFeeder(IEnumerable<Bowl> bowls, IInventory inventory, FoodId foodId)
        {
            List<Bowl> collection = bowls.ToList();
            _inventory = inventory;
            _bowls = new Queue<Bowl>(collection);
            _reservedBowls = new Stack<Bowl>();
            _foodId = foodId;

            Subscribe();

            _emptyBowls = new Queue<Bowl>(collection);
        }

        public bool TryGetFreeBowl(out Bowl bowl) =>
            _bowls.TryDequeue(out bowl);

        public void VacateBowl(Bowl bowl)
        {
            if (_bowls.Contains(bowl))
                throw new ArgumentOutOfRangeException(nameof(bowl));
            
            _bowls.Enqueue(bowl);
        }

        public bool HasFreeBowl(out Bowl bowl) =>
            _bowls.TryPeek(out bowl);

        public bool TryReserveBowl()
        {
            if (_bowls.TryDequeue(out Bowl bowl))
            {
                _reservedBowls.Push(bowl);
                return true;
            }

            return false;
        }

        public bool TryGetReservedBowl(out Bowl bowl) =>
            _reservedBowls.TryPop(out bowl);

        public void Dispose()
        {
            foreach (var action in _unsubscribeActions)
                action.Invoke();
            
            _inventory.Added -= OnAdded;
        }

        private void Subscribe()
        {
            _inventory.Added += OnAdded;

            foreach (var bowl in _bowls)
            {
                void OnFullBowl() => _emptyBowls.Dequeue();
                void OnEmptyBowl() => _emptyBowls.Enqueue(bowl);

                bowl.ProgressBarView.Full += OnFullBowl;
                bowl.ProgressBarView.Empty += OnEmptyBowl;
                
                _unsubscribeActions.Add(() =>
                {
                    bowl.ProgressBarView.Full -= OnFullBowl;
                    bowl.ProgressBarView.Empty -= OnEmptyBowl; 
                });
            }
        }

        private void OnAdded(IItem item) =>
            _emptyBowls.Peek().ReplenishFromInventory(item);
    }
}