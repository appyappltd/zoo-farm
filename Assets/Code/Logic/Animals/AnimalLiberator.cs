using Data.ItemsData;
using Logic.Spawners;
using Logic.Translators;
using UnityEngine;

[RequireComponent(typeof(RunTranslator))]
[RequireComponent(typeof(Delay))]
public class AnimalLiberator : MonoBehaviour
{
    private CollectibleCoinSpawner spawner;

    private void Awake()
    {
        spawner = GetComponent<CollectibleCoinSpawner>();

        GetComponent<Delay>().Complete += OnComplete;
    }

    private void OnComplete(GameObject player)
    {
        var inventory = player.GetComponent<Inventory>();

        if (inventory.CanGiveItem(CreatureType.Animal))
        {
            var item = inventory.Remove();
            var mover = item.GetComponent<IMover>();

            mover.Move(transform);
            mover.GotToPlace += () =>
            {
                spawner.Spawn(10);
                Destroy(item.gameObject);
            };
        }
    }
}
