using System.Collections;
using Data.ItemsData;
using Logic.Interactions;
using UnityEngine;

namespace Logic.Inventory
{
    [RequireComponent(typeof(TriggerObserver))]
    [RequireComponent(typeof(Delay))]
    public class ProductReceiver : MonoBehaviour
    {
        public bool CanTake = true;

        [SerializeField, Min(.0f)] private float _time = .2f;

        private Inventory inventory;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            GetComponent<Delay>().Complete += player => StartCoroutine(TryTakeItem(player));
            GetComponent<TriggerObserver>().Exit += _ => StopAllCoroutines();
        }

        private IEnumerator TryTakeItem(GameObject player)
        {
            if (CanTake)
            {
                var playerInventory = player.GetComponent<Inventory>();
                while (inventory.Type == CreatureType.None || playerInventory.CanGiveItem(inventory.Type) && inventory.CanAddItem(inventory.Type, playerInventory.GetData.Hand.Weight))
                {
                    inventory.Add(playerInventory.Remove());
                    yield return new WaitForSeconds(_time);
                }
            }
        }
    }
}
