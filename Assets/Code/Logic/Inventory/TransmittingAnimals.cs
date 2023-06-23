using Logic.Interactions;
using Logic.Volunteer;
using UnityEngine;

namespace Logic.Inventory
{
    [RequireComponent(typeof(Delay))]

    public class TransmittingAnimals : MonoBehaviour
    {
        [SerializeField] private VolunteerBand _band;

        private void Awake() => GetComponent<Delay>().Complete += TryTakeItem;

        private void TryTakeItem(GameObject player)
        {
            if (!_band.CanGiveAnimal())
                return;

            var inventory = player.GetComponent<Inventory>();
            if (inventory.GetCount > 0)
                return;

            inventory.Add(_band.GetAnimal());
        }
    }
}
