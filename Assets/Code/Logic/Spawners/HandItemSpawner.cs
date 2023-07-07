using Data.ItemsData;
using Infrastructure.Factory;
using Services;
using UnityEngine;

namespace Logic.Spawners
{
    public class HandItemSpawner : MonoBehaviour, ISpawner<HandItem>
    {
        [SerializeField] private Transform _spawnPlace;

        private IGameFactory _gameFactory;
        private IItemData _itemData;

        public void Construct(IItemData itemData)
        {
            _itemData = itemData;
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }
        
        public HandItem Spawn()
        {
            GameObject item = _gameFactory.CreateHandItem(_spawnPlace.position, _spawnPlace.rotation, _itemData.ItemId);
            HandItem handItem = item.GetComponent<HandItem>();
            handItem.Construct(_itemData);
            return handItem;
        }

        public void Dispose() { }
    }
}