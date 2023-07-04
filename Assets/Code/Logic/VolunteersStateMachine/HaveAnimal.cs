using Logic.Animals.AnimalsStateMachine.Transitions;
using Logic.Storages;
using StateMachineBase;
using UnityEngine;

namespace Logic.VolunteersStateMachine
{
    public class HaveAnimal : Transition
    {
        private Inventory inventory;
        public HaveAnimal(Inventory inventory) =>
            this.inventory = inventory;

        public override bool CheckCondition() =>
            inventory.Weight > 0;
    }
}
