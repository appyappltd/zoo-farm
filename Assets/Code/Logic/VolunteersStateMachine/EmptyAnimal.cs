using Logic.Storages;
using StateMachineBase;

namespace Logic.VolunteersStateMachine
{
    public class EmptyAnimal : Transition
    {
        private readonly IInventory _inventory;
        
        public EmptyAnimal(IInventory inventory) =>
            _inventory = inventory;
    
        public override bool CheckCondition() =>
            _inventory.Weight == 0;
    }
}
