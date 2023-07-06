using Logic.Storages;
using StateMachineBase;

namespace Logic.VolunteersStateMachine
{
    public class HaveAnimal : Transition
    {
        private readonly IInventory _inventory;
        
        public HaveAnimal(IInventory inventory) =>
            _inventory = inventory;

        public override bool CheckCondition() =>
            _inventory.Weight > 0;
    }
}
