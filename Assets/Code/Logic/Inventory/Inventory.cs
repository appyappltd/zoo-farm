using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Storage))]
public class Inventory : MonoBehaviour
{
    public event UnityAction<HandItem> AddItem;
    public event UnityAction<HandItem> RemoveItem;

    public HandItem GetLast => items.Last();
    public HandItem GetPreLast => items[^1];
    public int GetCount => items.Count;
    public Transform GetItemPlace => itemPlace;
    public CreatureType Type => currType;
    public ItemData GetData => items.First().ItemData;

    [field: SerializeField] public Transform DefItemPlace { get; private set; }
    [SerializeField] private CreatureType _inventoryType = CreatureType.None;
    [SerializeField, Min(0)] private int _maxAnimals = 1;
    [SerializeField, Min(0)] private int _maxMedical = 1;
    [SerializeField, Min(0)] private int _maxOther = 3;

    [SerializeField] private List<HandItem> items = new(); //Debag SerializeField
    private int maxCount = 1;
    private CreatureType currType = CreatureType.None;
    private Transform itemPlace = null;

    private void Awake()
    {
        itemPlace = DefItemPlace;

        if (_inventoryType != CreatureType.None)
        {
            SwitchMax(_inventoryType);
            currType = _inventoryType;
        }
    }

    public bool CanAddItem(CreatureType type) => maxCount > items.Count
                                              && (currType == CreatureType.None
                                              || currType == type);
    public bool CanGiveItem(CreatureType type) => items.Count > 0 
                                               && currType == type;
    public bool CanGiveItem() => items.Count > 0;

    public void Add(HandItem item)
    {
        if (items.Contains(item)) 
            throw new ArgumentException();
        if (currType == CreatureType.None)
            ChangeType(item.ItemData.Creature);
        items.Add(item);
        AddItem?.Invoke(item);
    }

    public HandItem Remove()
    {
        if (items.Count < 1)
            throw new ArgumentNullException();

        var item = items.Last();
        items.Remove(item);
        item.transform.SetParent(null);

        if (items.Count > 0)
            itemPlace = items.Last().NextPlace;
        else
        {
            itemPlace = DefItemPlace;
            currType = _inventoryType;
        }
        RemoveItem?.Invoke(item);

        return item;
    }

    private void SwitchMax(CreatureType type)
    {
        switch (type)
        {
            case CreatureType.Animal:
                maxCount = _maxAnimals;
                break;
            case CreatureType.Medical:
                maxCount = _maxMedical;
                break;
            case CreatureType.Other:
                maxCount = _maxOther;
                break;
        }
    }

    private void ChangeType(CreatureType type)
    {
        currType = type;
        SwitchMax(currType);
    }
}
