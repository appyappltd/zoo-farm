using System;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Medicine;
using Logic.Storages.Items;
using Services;
using StaticData;
using UnityEngine;

namespace Logic.Storages
{
    [RequireComponent(typeof(TimerOperator))]
    public class MedToolStand : MonoBehaviour, IGetItem, IGetItemProvider
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField, Min(.0f)] private float _respawnTime = 2f;

        private IGameFactory _gameFactory;
        private MedicineToolId _toolIdType;
        private IItem _toolItem;

        public event Action<IItem> Removed = i => { };

        public IGetItemObserver GetItemObserver => this;
        
        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_respawnTime, OnRespawn);
        }

        public void Construct(MedToolStandConfig config)
        {
            _toolIdType = config.Type;
            _icon.sprite = config.Icon;
            
            PlayRespawnDelay();
        }

        public IItem Get()
        {
            PlayRespawnDelay();
            IItem toolItem = _toolItem;
            _toolItem = null;
            Removed.Invoke(toolItem);
            return toolItem;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;

            if (_toolItem is not null && (_toolItem.ItemId & ItemId.Medical) > 0)
            {
                item = _toolItem;
                return true;
            }

            return false;
        }

        public bool TryGet(ItemId byId, out IItem result)
        {
            if (TryPeek(byId, out result))
            {
                result = Get();
                _toolItem = null;
                return true;
            }

            return false;
        }

        private void OnRespawn()
        {
            _toolItem = _gameFactory.CreateMedToolItem(_spawnPlace.position, Quaternion.identity, _toolIdType)
                .GetComponent<IItem>();
        }

        private void PlayRespawnDelay() =>
            _timerOperator.Restart();
    }
}