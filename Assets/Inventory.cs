using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public event UnityAction AddItem;
    public event UnityAction RemoveItem;

    public int GetCount => items.Count;
    [field: NonSerialized] public Transform DefItemPlace { get; private set; }

    [SerializeField, Min(1)] private int _maxAnimals = 1;
    [SerializeField, Min(1)] private int _maxMedical = 1;
    [SerializeField, Min(1)] private int _maxOther = 3;

    private List<HandItem> items = new();
    private int maxCount = 1;
    private CreatureType currType = CreatureType.None;
    private Transform itemPlace = null;

    private void Awake() => itemPlace = DefItemPlace;

    public bool CanAddItem(CreatureType type) => maxCount > items.Count
                                              && (currType == CreatureType.None
                                              || currType == type);
    public bool CanGiveItem(CreatureType type) => currType == type;
    public bool CanGiveItem() => items.Count > 0;

    public void Add(HandItem item)
    {
        if (items.Contains(item)) 
            throw new ArgumentException();
        if (currType == CreatureType.None)
            ChangeType(item.ItemData.Creature);
        items.Add(item);
        item.gameObject.transform.SetParent(DefItemPlace);
        item.GetComponent<Mover>().Move(itemPlace);
        itemPlace = item.NextPlace;
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
            currType = CreatureType.None;
        }
        return item;
    }

    private void SwitchMax()
    {
        switch (currType)
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
        SwitchMax();
    }
}
