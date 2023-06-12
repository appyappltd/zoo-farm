using Logic.Interactions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmittingAnimals : MonoBehaviour
{
    [SerializeField] private VolunteerBand _band;

    private void Awake() => GetComponent<TriggerObserver>().Enter += TryTakeItem;

    private void TryTakeItem(GameObject player)
    {
        if (!_band.CanGiveAnimal())
            return;

        var inventory = player.GetComponent<Inventory>();
        if (inventory.GetCount > 0)
            return;

        inventory.Add(_band.GetAnimal());
    }
}
