using System.Collections;
using Logic.Interactions;
using Logic.Movement;
using UnityEngine;

namespace Logic.Inventory
{
    public class Urn : MonoBehaviour
    {
        [SerializeField, Min(.0f)] private float _time = .2f;
        [SerializeField] private Transform _target;

        private void Awake()
        {
            GetComponent<TriggerObserver>().Enter += player => StartCoroutine(TryTakeItem(player));
            GetComponent<TriggerObserver>().Exit += _ => StopAllCoroutines();
        }

        private IEnumerator TryTakeItem(GameObject player)
        {
            var playerInventory = player.GetComponent<Inventory>();
            while (playerInventory.CanGiveItem())
            {
                var item = playerInventory.Remove();
                var mover = item.GetComponent<IMover>();

                mover.Move(_target);
                mover.GotToPlace += () => Destroy(item.gameObject);

                yield return new WaitForSeconds(_time);
            }
        }
    }
}
