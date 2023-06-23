using System.Collections;
using System.Collections.Generic;
using Data.ItemsData;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Storage))]
public class ItemRespawner : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _respawnTime = 5f;
    [SerializeField] private HandItem _item;

    private Inventory inventory;
    private Storage storage;

    private void Awake()
    {
        storage = GetComponent<Storage>();
        inventory = GetComponent<Inventory>();
        inventory.RemoveItem += _ =>
        {
            StartCoroutine(RespawnItem());
        };
        StartCoroutine(RespawnItem());
    }

    private IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(_respawnTime);
        inventory.Add(Instantiate(_item, storage.GetItemPlace.position, storage.GetItemPlace.rotation));
    }
}
