using Data.ItemsData;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic.Storages
{
    [RequireComponent(typeof(TimerOperator))]
    public class ItemRespawner : MonoBehaviour
    {
        [SerializeField, Min(.0f)] private float _respawnTime = 2f;
        [SerializeField] private HandItem _itemPrefab;
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private InventoryProvider _inventoryProvider;
        [SerializeField] private Storage _storage;

        private IInventory _inventory;
        
        private void Awake()
        {
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_respawnTime, OnRespawn);
            _inventory = _inventoryProvider.Inventory;
            PlayRespawnDelay();
        }

        private void OnEnable() =>
            _inventory.Removed += OnItemTaken;

        private void OnDisable() =>
            _inventory.Removed -= OnItemTaken;

        private void OnRespawn()
        {
            HandItem handItem = Instantiate(_itemPrefab, _storage.TopPlace.position, Quaternion.identity);
            _inventory.Add(handItem);
        }

        private void OnItemTaken(IItem _) =>
            PlayRespawnDelay();

        private void PlayRespawnDelay() =>
            _timerOperator.Restart();
    }
}