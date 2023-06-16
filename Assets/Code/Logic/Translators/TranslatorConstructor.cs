using System;
using Infrastructure.Factory;
using Logic.Spawners;
using NaughtyAttributes;
using NTC.Global.Cache;
using Services;
using Tools;
using UnityEngine;

namespace Logic.Translators
{
    public class TranslatorConstructor : MonoCache
    {
        [SerializeField] [RequireInterface(typeof(ITranslator))]
        private MonoBehaviour _translator;

        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;

        private VisualTranslatorsSpawner _spawner;

        private ITranslator Translator => (ITranslator) _translator;

        private void Awake()
        {
            _spawner = new VisualTranslatorsSpawner()
            
            _spawner = new VisualTranslatorsSpawner(
                (() => AllServices.Container.Single<IGameFactory>()
                    .CreateVisual(VisualType.Money, Quaternion.identity, transform)),
                Translator,
                4,
                _from,
                _to);
        }

        private void OnDestroy()
        {
            _spawner.Dispose();
        }

        [Button("Translate")]
        private void Translate()
        {
            _spawner.Spawn();
        }
    }
}