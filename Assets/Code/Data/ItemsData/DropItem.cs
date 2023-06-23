using Logic.Interactions;
using Logic.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Data.ItemsData
{
    [RequireComponent(typeof(TriggerObserver))]
    [RequireComponent(typeof(Delay))]

    public class DropItem : MonoBehaviour
    {
        public event UnityAction<HandItem> PickUp;

        [field: SerializeField] public ItemData ItemData { get; private set; }

        private void Awake()
        {
            GetComponent<Delay>().Complete += player =>
            {
                var inventory = player.GetComponent<Inventory>();

                if (inventory.CanAddItem(ItemData.Creature, ItemData.Hand.Weight))
                {
                    var item = Instantiate(ItemData.Hand, transform.position,
                                                          transform.rotation);
                    inventory.Add(item);
                    PickUp?.Invoke(item);
                    Destroy(gameObject, .01f);
                }
            };
        }
    }
}
