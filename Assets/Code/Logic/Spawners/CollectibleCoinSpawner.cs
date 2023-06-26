using System;
using Infrastructure.Factory;
using Logic.Movement;
using Logic.Translators;
using Services;
using Tools.Extension;
using Unity.VisualScripting;
using UnityEngine;

namespace Logic.Spawners
{
    [RequireComponent(typeof(TimerOperator))]
    [RequireComponent(typeof(RunTranslator))]
    public class CollectibleCoinSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _coinsSpawnZone;
        [SerializeField] private Vector2 _zoneOffset;
        [SerializeField] private float _spawnDelay;

        private PooledSpawner<TranslatableAgent> _pooledSpawner;
        private ITranslator _translator;
        private TimerOperator _timerOperator;
        private int _remainingCountCoins;

        private void Awake()
        {
            _translator = GetComponent<ITranslator>();
            _timerOperator = GetComponent<TimerOperator>();
            _timerOperator.SetUp(_spawnDelay, SpawnCoin);
            Transform container = new GameObject("Collectible Coins Container").transform;

            _pooledSpawner = new PooledSpawner<TranslatableAgent>(
                () =>
                    AllServices.Container.Single<IGameFactory>()
                        .CreateCollectibleCoin(container),
            10, OnReturnToPool);
        }

        private void OnDestroy() =>
            _pooledSpawner.Dispose();

        private void OnDrawGizmos()
        {
            if (_coinsSpawnZone.IsUnityNull())
            {
                return;
            }
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(_coinsSpawnZone.position, _zoneOffset.AddY(0.1f) * 2);
        }

        public void Spawn(int amountCoins)
        {
            _remainingCountCoins = amountCoins;
            _timerOperator.Restart();
        }

        private void SpawnCoin()
        {
            TranslatableAgent agent = _pooledSpawner.Spawn();
            PlayTranslatables(agent);
            AddToTranslator(agent);
            _remainingCountCoins--;

            if (_remainingCountCoins > 0)
                _timerOperator.Restart();
        }

        private Action OnReturnToPool(Action returnAction, TranslatableAgent coin)
        {
            void OnEndMove() => returnAction.Invoke();

            TowardsMover towardsMover = coin.GetComponent<TowardsMover>();
            towardsMover.GotToPlace += OnEndMove;
            return () => towardsMover.GotToPlace -= OnEndMove;
        }

        private void PlayTranslatables(TranslatableAgent agent)
        {
            ITranslatableParametric<Vector3> mainTranslatable = (ITranslatableParametric<Vector3>) agent.MainTranslatable;
            mainTranslatable.Play(transform.position, GetRandomAroundPosition());

            for (var index = 0; index < agent.SubTranslatables.Count; index++)
            {
                ITranslatable translatable = agent.SubTranslatables[index];
                translatable.Play();
            }
        }

        private Vector3 GetRandomAroundPosition() =>
            _coinsSpawnZone.position.GetRandomAroundPosition(new Vector3(_zoneOffset.x, 0, _zoneOffset.y));

        private void AddToTranslator(TranslatableAgent agent)
        {
            _translator.AddTranslatable(agent.MainTranslatable);

            foreach (ITranslatable translatable in agent.SubTranslatables)
            {
                _translator.AddTranslatable(translatable);
            }
        }
    }
}