using System;
using Logic.Player;
using Logic.Storages;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class ProductReceiverValidator : MonoBehaviour, IInteractionValidator
    {
        [SerializeField] private ProductReceiver _productReceiver;

        public bool IsValid<T>(T target = default)
        {
            if (target is IHuman human)
                return _productReceiver.CanReceive(human.Inventory);

            throw new ArgumentOutOfRangeException(nameof(target));
        }
    }
}