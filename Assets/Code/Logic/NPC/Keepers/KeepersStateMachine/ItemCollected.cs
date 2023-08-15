using Logic.Storages;
using Logic.Storages.Items;
using StateMachineBase;

namespace Code.Logic.NPC.Keepers.KeepersStateMachine
{
    internal class ItemCollected : Transition
    {
        private readonly IInventory _inventory;

        private bool _isAdded;
        
        public ItemCollected(IInventory inventory)
        {
            _inventory = inventory;
        }

        public override void Enter()
        {
            _isAdded = false;
            _inventory.Added += OnAdded;
        }

        public override void Exit()
        {
            _inventory.Added -= OnAdded;
        }

        public override bool CheckCondition() =>
            _isAdded;

        private void OnAdded(IItem obj) =>
            _isAdded = true;
    }
}