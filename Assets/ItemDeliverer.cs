using UnityEngine;

[RequireComponent(typeof(Delay))]
public class ItemDeliverer : MonoBehaviour
{
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        GetComponent<Delay>().Complete += PassItem;
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
