using Logic.Interactions;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MedicineBed : MonoBehaviour
{
    [SerializeField] private List<ItemData> _data = new();
    [SerializeField] private List<Sprite> _sprites = new();

    private Inventory inventory;
    private int index = 0;
    private bool CanTreat = false;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        GetComponent<TriggerObserver>().Enter += player => Treat(player.GetComponent<Inventory>());

        inventory.AddItem += item => item.GetComponent<Mover>().GotToPlace += () =>
        {
            CanTreat = true;
            GetRandomIndex();
            item.GetComponent<BubbleHolder>().GetBubble.ChangeState(_sprites[index]);
        };
    }

    private void GetRandomIndex() => index = Random.Range(0, _data.Count);

    private void Treat(Inventory playerInventory)
    {
        if (!CanTreat) 
            return;
        if (!inventory.CanGiveItem())
            return;
        if (!playerInventory.CanGiveItem(_data[index].Creature))
            return;

        var item = playerInventory.Remove();
        var mover = item.GetComponent<Mover>();
        var handAnimal = inventory.Remove();

        mover.Move(inventory.DefItemPlace);
        mover.GotToPlace += () =>
        {
            Instantiate(handAnimal.ItemData.Drop, handAnimal.transform.position,
                                                  handAnimal.transform.rotation);

            Destroy(handAnimal.GetComponent<BubbleHolder>().GetBubble.gameObject);
            Destroy(handAnimal);
            Destroy(item.gameObject);
        };
        CanTreat = false;
    }
}
