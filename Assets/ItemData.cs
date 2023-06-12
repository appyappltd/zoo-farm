using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "itemData")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public HandItem Hand { get; private set; }
    [field: SerializeField] public DropItem Drop { get; private set; }
    [field: SerializeField] public CreatureType Creature { get; private set; }
}
