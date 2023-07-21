using Logic.Storages;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class ProductReceiverValidator : MonoBehaviour, IInteractionValidator
    {
        [SerializeField] private ProductReceiver _productReceiver;

        public bool IsValid(IInventory inventory = default) =>
            _productReceiver.CanReceive(inventory);
    }
}