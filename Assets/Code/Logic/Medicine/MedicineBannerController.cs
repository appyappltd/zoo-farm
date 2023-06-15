using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineBannerController : MonoBehaviour
{
    [SerializeField] private GameObject _banner;

    private void Awake()
    {
        _banner.SetActive(false);

        var inventory = GetComponent<Inventory>();
        inventory.AddItem += _ => _banner.SetActive(true);
        inventory.RemoveItem += () => _banner.SetActive(false);
    }
}
