using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Animator))]
public class HandsAnimator : MonoBehaviour
{
    private Inventory inventory;
    private Animator animC;
    public  bool Debug;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        animC = GetComponent<Animator>();

        inventory.AddItem += _ => ChangeHandsState();
        inventory.RemoveItem += _ => ChangeHandsState();
    }

    private void ChangeHandsState()
    {
        Debug = inventory.GetCount > 0;
        animC.SetBool("IsHold", Debug);
    }
}
