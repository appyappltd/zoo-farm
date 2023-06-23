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

    private void Awake()
    {
        inventory = GetComponent<Inventory>();

        inventory.AddItem += PlaceItem;
        inventory.RemoveItem += RevertItem;
    }

    private void PlaceItem(HandItem item)
    {
        var mover = item.GetComponent<IMover>();
        item.transform.SetParent(_places[0]);

        mover.Move(_places.Length < inventory.GetCount
                  ? inventory.GetPreLast.NextPlace
                  : _places[inventory.GetCount - 1]);
    }

    private void RevertItem(HandItem item)
    {
        item.transform.SetParent(null);
    }
}
