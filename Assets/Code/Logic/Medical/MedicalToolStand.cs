using System;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Storages;
using Logic.Storages.Items;
using Services;
using Services.MedicalBeds;
using StaticData;
using UnityEngine;

namespace Logic.Medical
{
    [RequireComponent(typeof(TimerOperator))]
    public class MedicalToolStand : MonoBehaviour, IGetItem
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField, Min(.0f)] private float _respawnTime = 2f;

        private IGameFactory _gameFactory;
        private IMedicalBedsReporter _needsReporter;
        private IItem _toolItem;
        private MedicalToolId _toolId;

        public event Action<IItem> Removed = i => { };
        
        public MedicalToolId ToolId => _toolId;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_respawnTime, OnRespawn);
        }

        public void Construct(MedToolStandConfig config)
        {
            _toolId = config.Type;
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

            if (_toolItem is null || (_toolItem.ItemId & ItemId.Medical) <= 0)
                return false;
            
            item = _toolItem;
            return true;

        }

        public bool TryGet(ItemId byId, out IItem result)
        {
            if (TryPeek(byId, out result))
            {
                result = Get();
                return true;
            }

            return false;
        }

        private void OnRespawn()
        {
            _toolItem = _gameFactory.CreateMedToolItem(_spawnPlace.position, Quaternion.identity, ToolId)
                .GetComponent<IItem>();
        }

        private void PlayRespawnDelay() =>
            _timerOperator.Restart();
    }
}