using Logic.Interactions;
using System.Collections;
using Data.ItemsData;
using UnityEngine;

[RequireComponent(typeof(TriggerObserver))]
[RequireComponent(typeof(Delay))]
public class ProductReceiver : MonoBehaviour
{
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
        var playerInventory = player.GetComponent<Inventory>();
        while (_type == CreatureType.None || playerInventory.CanGiveItem(_type) && inventory.CanAddItem(_type))
        {
            inventory.Add(playerInventory.Remove());
            yield return new WaitForSeconds(_time);
        }
    }
}
