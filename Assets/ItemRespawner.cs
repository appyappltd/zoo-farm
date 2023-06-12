using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawner : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _respawnTime = 5f;
    [SerializeField] private HandItem _item;
    private Inventory inventory;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        inventory.RemoveItem += () =>
        {
            StartCoroutine(RespawnItem());

        };
        StartCoroutine(RespawnItem());
    }

    private IEnumerator RespawnItem()
    {
        yield return new WaitForSeconds(_respawnTime);
        inventory.Add(Instantiate(_item, inventory.GetItemPlace.position, inventory.GetItemPlace.rotation));
    }
}
