using System.Collections.Generic;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.AnimalsBehaviour;
using Services;
using Services.AnimalHouse;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Delay))]
public class MedicineBed : MonoBehaviour
{
    [SerializeField] private List<ItemData> _data = new();
    [SerializeField] private List<Sprite> _sprites = new();

    private Inventory inventory;
    private ProductReceiver receiver;
    private int index = 0;
    private bool canTreat = false;

    private IGameFactory gameFactory;
    private IAnimalHouseService houseService;

    private void Awake()
    {
        gameFactory = AllServices.Container.Single<IGameFactory>();
        houseService = AllServices.Container.Single<IAnimalHouseService>();

        inventory = GetComponent<Inventory>();
        receiver = GetComponent<ProductReceiver>();
        GetComponent<Delay>().Complete += player => Treat(player.GetComponent<Inventory>());

        inventory.AddItem += item => item.GetComponent<Mover>().GotToPlace += () =>
        {
            canTreat = true;
            receiver.canTake = false;
            GetRandomIndex();
            item.GetComponent<BubbleHolder>().GetBubble.ChangeState(_sprites[index]);
        };
    }

    private void GetRandomIndex() => index = Random.Range(0, _data.Count);

    private void Treat(Inventory playerInventory)
    {
        if (!canTreat)
            return;
        if (!inventory.CanGiveItem())
            return;
        if (!playerInventory.CanGiveItem(_data[index].Creature))
            return;
        if (playerInventory.GetDataFirstElement.Hand.GetComponent<Medicine>().Type !=
                  _data[index].Hand.GetComponent<Medicine>().Type)
            return;

        var item = playerInventory.Remove();
        var mover = item.GetComponent<Mover>();
        var handAnimal = inventory.Remove();

        mover.MoveTowards(inventory.DefItemPlace);
        mover.GotToPlace += () =>
        {
            Debug.Log("A");
            houseService.TakeQueueToHouse(() =>
            {
                var animalItemData = (AnimalItemData)handAnimal.ItemData;
                canTreat = false;
                receiver.canTake = true;

                Destroy(handAnimal.gameObject);
                return gameFactory.CreateAnimal(animalItemData.AnimalType, handAnimal.transform.position)
                    .GetComponent<Animal>();
            });

            Destroy(handAnimal.GetComponent<BubbleHolder>().GetBubble.gameObject);
            Destroy(item.gameObject);
        };
    }
}
