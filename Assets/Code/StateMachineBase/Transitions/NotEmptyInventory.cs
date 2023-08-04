using Logic.Storages;

namespace StateMachineBase.Transitions
{
    public class NotEmptyInventory : Transition
    {
        private readonly IInventory _inventory;

        public NotEmptyInventory(IInventory inventory) =>
            _inventory = inventory;

        public override bool CheckCondition() =>
            !_inventory.IsEmpty;
    }
}