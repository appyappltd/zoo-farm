using Data.ItemsData;
using Logic.Interactions;
using Logic.Movement;
using Logic.Spawners;
using Logic.Translators;
using UnityEngine;

namespace Logic.Animals
{
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
            var inventory = player.GetComponent<Inventory.Inventory>();

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
}
