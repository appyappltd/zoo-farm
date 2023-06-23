using Data.ItemsData;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

[RequireComponent(typeof(Inventory))]
public class Storage : MonoBehaviour
{
    public Transform GetItemPlace => _places.Length < inventory.GetCount
                                   ? inventory.GetLast.NextPlace
                                   : _places[inventory.GetCount];

    [SerializeField] private Transform[] _places;

    private Inventory inventory;
    private HandItem currItem;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        inventory.AddItem += PlaceItem;
        inventory.RemoveItem += RevertItem;
    }

    private void PlaceItem(HandItem item)
    {
        currItem = item;
        var mover = currItem.GetComponent<IMover>();

        mover.Move(_places.Length < inventory.GetCount
                  ? inventory.GetPreLast.NextPlace
                  : _places[inventory.GetCount - 1]);
        mover.GotToPlace += SetParent;
    }

    private void SetParent()
    {
        currItem.transform.SetParent(_places[0]);
        currItem.GetComponent<IMover>().GotToPlace -= SetParent;
    }

    private void RevertItem(HandItem item)
    {
        item.transform.SetParent(null);
    }
}
