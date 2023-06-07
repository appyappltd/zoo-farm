using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItem : MonoBehaviour
{
    [field: NonSerialized] public ItemData ItemData { get; private set; }
    [field: NonSerialized] public Transform NextPlace { get; private set; }
}
