using StateMachineBase;

namespace Logic.VolunteersStateMachine
{
    public class EmptyAnimal : Transition
    {
        private Inventory.Inventory inventory;
        public EmptyAnimal(Inventory.Inventory inventory) =>
            this.inventory = inventory;
    
        public override bool CheckCondition() =>
            inventory.Weight == 0;
    }
}
