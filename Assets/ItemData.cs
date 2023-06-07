using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "itemData")]
public class ItemData : ScriptableObject
{
    [field: NonSerialized] public HandItem Hand { get; private set; }
    [field: NonSerialized] public DropItem Drop { get; private set; }
    [field: NonSerialized] public CreatureType Creature { get; private set; }
}
