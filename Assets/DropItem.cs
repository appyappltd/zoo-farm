using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    [field: NonSerialized] public ItemData ItemData { get; private set; }
}
