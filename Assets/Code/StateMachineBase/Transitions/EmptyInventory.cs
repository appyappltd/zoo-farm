using Logic.Storages;

namespace StateMachineBase.Transitions
{
    public class EmptyInventory : Transition
    {
        private readonly IInventory _inventory;

        public EmptyInventory(IInventory inventory) =>
            _inventory = inventory;

        public override bool CheckCondition() =>
            _inventory.IsEmpty;
    }
}