using Logic.Interactions;
using System.Collections;
using Data.ItemsData;
using UnityEngine;

public class ProductReceiver : MonoBehaviour
{
    [SerializeField] private CreatureType _type;
    [SerializeField, Min(.0f)] private float _time = .2f;

    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        GetComponent<TriggerObserver>().Enter += player => StartCoroutine(TryTakeItem(player));
        GetComponent<TriggerObserver>().Exit += _ => StopAllCoroutines();
    }

    private IEnumerator TryTakeItem(GameObject player)
    {
        var playerInventory = player.GetComponent<Inventory>();
        while (_type == CreatureType.None || playerInventory.CanGiveItem(_type) && inventory.CanAddItem(_type))
        {
            inventory.Add(playerInventory.Remove());
            yield return new WaitForSeconds(_time);
        }
    }
}
