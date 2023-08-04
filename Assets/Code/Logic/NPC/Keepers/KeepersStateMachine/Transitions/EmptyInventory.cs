using Logic.Storages;
using StateMachineBase;

namespace Logic.NPC.Keepers.KeepersStateMachine.Transitions
{
    internal class EmptyInventory : Transition
    {
        private readonly IInventory _inventory;

        public EmptyInventory(IInventory inventory) =>
            _inventory = inventory;

        public override bool CheckCondition() =>
            _inventory.IsEmpty;
    }
}