using Logic.Inventory;
using StateMachineBase;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveAnimal : Transition
{
    private Inventory inventory;
    public HaveAnimal(Inventory inventory) =>
        this.inventory = inventory;

    public override bool CheckCondition() =>
        inventory.Weight > 0;
}
