using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Storages
{
    public class Trash : MonoBehaviour
    {
        [SerializeField] private Storage _storage;

        private void OnEnable()
        {
           _storage.Replenished += OnReplenished;
        }

        private void OnReplenished(IItem item)
        {
            item.Mover.Ended += OnMoveEnded;

            void OnMoveEnded()
            {
                item.Destroy();
                item.Mover.Ended -= OnMoveEnded;
            }
        }
    }
}