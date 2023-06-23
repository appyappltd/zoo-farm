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
    public event UnityAction<HandItem> AddItem = c => { };
    public event UnityAction<HandItem> RemoveItem = c => { };

    public HandItem GetLast => items.Last();
    public HandItem GetPreLast => items[^2];
    public int GetCount => items.Count;
    public CreatureType Type => currType;
    public ItemData GetData => items.Last().ItemData;

    [SerializeField] private CreatureType _type = CreatureType.None;
    [SerializeField, Min(0)] private int _maxWeight = 3;

    private List<HandItem> items = new();
    private int weight = 0;
    private CreatureType currType = CreatureType.None;

    private void Awake()
    {
        currType = _type;
    }

    public bool CanAddItem(CreatureType type, int weight) => _maxWeight >= this.weight + weight
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
        ChangeWeight(item.Weight);

        AddItem.Invoke(item);
    }

    public HandItem Remove()
    {
        if (items.Count < 1)
            throw new ArgumentNullException();
        var item = items.Last();

        items.Remove(item);
        ChangeWeight(-item.Weight);

        RemoveItem.Invoke(item);

        if (items.Count == 0)
            ChangeType(_type);
        return item;
    }

    private void ChangeType(CreatureType type)
    {
        currType = type;
    }

    private void ChangeWeight(int amount)
    {
        weight += amount;
    }
}
