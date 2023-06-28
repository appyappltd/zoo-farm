using StateMachineBase;

namespace Logic.VolunteersStateMachine
{
    public class HaveAnimal : Transition
    {
        private Inventory.Inventory inventory;
        public HaveAnimal(Inventory.Inventory inventory) =>
            this.inventory = inventory;

        public override bool CheckCondition() =>
            inventory.Weight > 0;
    }
}
