using Data.ItemsData;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialGiveItemTriggerContainer : MonoBehaviour
    {
        [SerializeField] private DropItem _dropItem;
        [SerializeField] private TutorialTriggerStatic _triggerStatic;

        private void OnEnable() =>
            _dropItem.PickUp += OnPickUp;

        private void OnDisable() =>
            _dropItem.PickUp -= OnPickUp;

        private void OnPickUp(HandItem item) =>
            _triggerStatic.Trigger(gameObject);
    }
}