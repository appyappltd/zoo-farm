using System;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Storages;
using Logic.Storages.Items;
using Services;
using StaticData;
using UnityEngine;

namespace Logic.Medicine
{
    [RequireComponent(typeof(TimerOperator))]
    public class MedToolStand : MonoBehaviour, IGetItem, IGetItemProvider
    {
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField, Min(.0f)] private float _respawnTime = 2f;

        private IGameFactory _gameFactory;
        private MedicalToolNeedsReporter _needsReporter;
        private IItem _toolItem;
        private MedicineToolId _toolId;
        private bool _isNeeded;

        public event Action<IItem> Removed = i => { };

        public IGetItem ItemGetter => this;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _timerOperator ??= GetComponent<TimerOperator>();
            _timerOperator.SetUp(_respawnTime, OnRespawn);
        }

        private void OnDestroy() =>
            _needsReporter.ToolNeeds -= OnToolNeeds;

        public void Construct(MedToolStandConfig config, MedicalToolNeedsReporter needsReporter)
        {
            _needsReporter = needsReporter;
            _toolId = config.Type;
            _icon.sprite = config.Icon;
            _needsReporter.ToolNeeds += OnToolNeeds;
            _isNeeded = _needsReporter.IsNeeds(_toolId);
            
            PlayRespawnDelay();
        }

        public IItem Get()
        {
            PlayRespawnDelay();
            IItem toolItem = _toolItem;
            _toolItem = null;
            _isNeeded = false;
            Removed.Invoke(toolItem);
            return toolItem;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;
            
            if (_isNeeded == false)
                return false;

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
                _toolItem = null;
                return true;
            }

            return false;
        }

        private void OnToolNeeds(MedicineToolId toolId) =>
            _isNeeded = toolId == _toolId;

        private void OnRespawn()
        {
            _toolItem = _gameFactory.CreateMedToolItem(_spawnPlace.position, Quaternion.identity, _toolId)
                .GetComponent<IItem>();
        }

        private void PlayRespawnDelay() =>
            _timerOperator.Restart();
    }
}