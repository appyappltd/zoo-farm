using Logic.Storages;
using Logic.Storages.Items;
using Tools;
using UnityEngine;

namespace Tutorial
{
    public class TutorialGiveItemTriggerContainer : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(IGetItemObserver))] private MonoBehaviour _getter;
        [SerializeField] private StaticTriggers.TutorialTriggerStatic _triggerStatic;

        private void OnEnable() =>
            ((IGetItemObserver) _getter).Removed += OnGet;

        private void OnDisable() =>
            ((IGetItemObserver) _getter).Removed -= OnGet;

        private void OnGet(IItem obj) =>
            _triggerStatic.Trigger(gameObject);
    }
}