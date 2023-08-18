using Logic.Storages;
using Logic.Storages.Items;
using NTC.Global.System;
using UnityEngine;

namespace Logic
{
    public class MaxIndicator : MonoBehaviour
    {
        [SerializeField] private GameObject _indicator;
        [SerializeField] private Storage _storage;

        private IInventory _inventory;

        private void OnDestroy()
        {
            if (_inventory is null)
                return;

            _inventory.Added -= ChangeBubbleState;
            _inventory.Removed -= ChangeBubbleState;
        }

        public void Construct(IInventory inventory)
        {
            _inventory = inventory;
            _inventory.Added += ChangeBubbleState;
            _inventory.Removed += ChangeBubbleState;
            _indicator.gameObject.SetActive(false);
        }

        private void ChangeBubbleState(IItem _)
        {
            if (_inventory.IsFull)
            {
                _indicator.transform.SetParent(_storage.TopPlace);
                _indicator.transform.localPosition = Vector3.zero;
                _indicator.gameObject.Enable();
            }
            else
            {
                _indicator.gameObject.Disable();
            }
        }
    }
}