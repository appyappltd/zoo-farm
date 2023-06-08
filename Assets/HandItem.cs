using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandItem : MonoBehaviour
{
    [field: SerializeField] public ItemData ItemData { get; private set; }
    [field: SerializeField] public Transform NextPlace { get; private set; }
}
