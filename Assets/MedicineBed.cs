using Logic.Interactions;
using System.Collections.Generic;
using UnityEngine;

public class MedicineBed : MonoBehaviour
{
    [SerializeField] private List<ItemData> _data = new ();
    [SerializeField] private List<SpriteRenderer> _sprites = new();

    private Inventory inventory;
    private int index = 0;

    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        GetComponent<TriggerObserver>();

        inventory.AddItem += item =>
        {

        };
    }

    private void GetRandomIndex()
    {
        index = Random.Range(0, _data.Count);
    }

    private void TakeMedicine()
    {

    }
}
