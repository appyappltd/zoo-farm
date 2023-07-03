using Data.ItemsData;
using Logic.Bubble;
using Logic.Inventory;
using UnityEngine;

public class MaxIndicator : MonoBehaviour
{
    [SerializeField] private Bubble bubble;

    private Inventory inventory;

    private void Start()
    {
        inventory = GetComponent<Inventory>();

        inventory.AddItem += ChangeBubbleState;
        inventory.RemoveItem += ChangeBubbleState;
        bubble.gameObject.SetActive(false);
    }

    private void ChangeBubbleState(HandItem _) =>
        bubble.gameObject.SetActive(inventory.IsMax);
}
