using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Bubble
{
    public class MaxIndicator : MonoBehaviour
    {
        [SerializeField] private Bubble bubble;

        private Inventory inventory;

        private void Start()
        {
            inventory = GetComponent<Inventory>();

            inventory.Added += ChangeBubbleState;
            inventory.Removed += ChangeBubbleState;
            bubble.gameObject.SetActive(false);
        }

        private void ChangeBubbleState(IItem _) =>
            bubble.gameObject.SetActive(inventory.IsFull);
    }
}
