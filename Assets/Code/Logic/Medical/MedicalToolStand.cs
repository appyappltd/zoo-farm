using System;
using AYellowpaper;
using Infrastructure.Factory;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.Translators;
using Services;
using Services.MedicalBeds;
using StaticData;
using UnityEngine;

namespace Logic.Medical
{
    [RequireComponent(typeof(TimerOperator))]
    public class MedicalToolStand : MonoBehaviour, IGetItem, IAddItemObserver
    {
        [SerializeField] private InterfaceReference<ITranslator, MonoBehaviour> _translator;
        [SerializeField] private TimerOperator _timerOperator;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] [Min(.0f)] private float _respawnTime = 2f;

        private IHandItemFactory _handItemFactory;
        private IMedicalBedsReporter _medicalBedsReporter;
        private IItem _toolItem;
        private TreatToolId _toolId;

        public event Action<IItem> Removed = _ => { };
        public event Action<IItem> Added = _ => { };

        public TreatToolId ToolId => _toolId;

        private void Awake()
        {
            _handItemFactory = AllServices.Container.Single<IGameFactory>().HandItemFactory;
            _medicalBedsReporter = AllServices.Container.Single<IMedicalBedsReporter>();
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

        public bool TryPeek(ItemFilter filter, out IItem item)
        {
            item = null;

            bool isCanPeek = _toolItem is not null && _medicalBedsReporter.IsNeeds(_toolId);

            if (isCanPeek)
            {
                item = _toolItem;
                return true;
            }

            return false;
        }

        public bool TryGet(ItemFilter filter, out IItem result)
        {
            if (TryPeek(filter, out result))
            {
                result = Get();
                return true;
            }

            return false;
        }

        private void OnRespawn()
        {
            _toolItem = _handItemFactory.CreateMedicalToolItem(_spawnPlace.position, Quaternion.identity, _toolId);
            
            if (_toolItem is AnimatedItem animatedItem)
            {
                animatedItem.ScaleTranslatable.Play(Vector3.zero, Vector3.one);
                _translator.Value.Add(animatedItem.ScaleTranslatable);
            }
            
            Added.Invoke(_toolItem);
        }

        private void PlayRespawnDelay() =>
            _timerOperator.Restart();
    }
}