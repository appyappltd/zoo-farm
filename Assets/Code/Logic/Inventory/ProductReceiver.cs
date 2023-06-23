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
        public bool canTake = true;

        [SerializeField] private CreatureType _type;
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
            if (canTake)
            {
                var playerInventory = player.GetComponent<Inventory>();
                while (_type == CreatureType.None || playerInventory.CanGiveItem(_type) && inventory.CanAddItem(_type,playerInventory.GetData.Hand.Weight))
                {
                    inventory.Add(playerInventory.Remove());
                    yield return new WaitForSeconds(_time);
                }
            }
        }
    }
}
