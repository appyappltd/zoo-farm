using Logic.Storages;
using StateMachineBase;

namespace Logic.VolunteersStateMachine
{
    public class EmptyAnimal : Transition
    {
        private Inventory inventory;
        public EmptyAnimal(Inventory inventory) =>
            this.inventory = inventory;
    
        public override bool CheckCondition() =>
            inventory.Weight == 0;
    }
}
