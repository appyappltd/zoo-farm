using Logic.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

public class ItemDeliverer : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        GetComponent<TriggerObserver>().Enter += PassItem;
    }

    private void PassItem(GameObject player)
    {
        if (!inventory.CanGiveItem())
            return;
        var playerInventory = player.GetComponent<Inventory>();
        if (playerInventory.CanAddItem(inventory.Type))
            playerInventory.Add(inventory.Remove());
    }
}
