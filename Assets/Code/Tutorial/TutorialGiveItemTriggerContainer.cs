using Data.ItemsData;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialGiveItemTriggerContainer : MonoBehaviour
    {
        [SerializeField] private HandItem _item;
        [SerializeField] private TutorialTriggerStatic _triggerStatic;

        //TODO: Заменить подписку на ивент взятия в классе грядки
        
        private void OnEnable() =>
            _item.Mover.Ended += OnPickUp;

        private void OnDisable() =>
            _item.Mover.Ended -= OnPickUp;

        private void OnPickUp() =>
            _triggerStatic.Trigger(gameObject);
    }
}