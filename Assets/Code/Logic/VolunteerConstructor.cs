using Logic.Storages;
using UnityEngine;

namespace Logic
{
    public class VolunteerConstructor : MonoBehaviour
    {
        [SerializeField] private Volunteer.Volunteer _volunteer;
        [SerializeField] private Storage _storage;

        private void Awake() =>
            _storage.Construct(_volunteer.Inventory, _volunteer.Inventory);
    }
}