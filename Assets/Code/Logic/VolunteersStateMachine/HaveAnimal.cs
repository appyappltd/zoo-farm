using Logic.Animals.AnimalsStateMachine.Transitions;
using StateMachineBase;
using UnityEngine;

namespace Logic.VolunteersStateMachine
{
    public class HaveAnimal : TargetInRange
    {
        private Inventory.Inventory inventory;
        public HaveAnimal(Transform origin, Transform target, float range,
                          Inventory.Inventory inventory) : base(origin, target, range) =>
            this.inventory = inventory;

        public override bool CheckCondition() =>
            !base.CheckCondition() && inventory.Weight > 0;
    }
}
