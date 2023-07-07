using Logic.Storages;
using Logic.VolunteersStateMachine;
using UnityEngine;

namespace Logic.Volunteer
{
    public class Volunteer : MonoBehaviour
    {
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private VolunteerStateMachine _stateMachine;
        [SerializeField] private HandsAnimator _handsAnimator;
        [SerializeField] private Storage _storage;

        public IInventory Inventory => _inventoryHolder.Inventory;
        public VolunteerStateMachine StateMachine => _stateMachine;
        public bool IsFree => _inventoryHolder.Inventory.Weight <= 0;

        public void Awake()
        {
            _inventoryHolder.Construct();
            _handsAnimator.Construct(_inventoryHolder.Inventory);
            _storage.Construct(_inventoryHolder.Inventory);
        }
    }
}