using System;
using Logic.Translators;
using UnityEngine;

namespace Pool
{
    public class VisualTranslatorsSpawner : IDisposable
    {
        private readonly ITranslator _translator;
        private readonly string _poolContainerName;
        private readonly int _preloadCount;
        private readonly Transform _fromTransform;
        private readonly Transform _toTransform;

        private Transform _container;
        private Pool<TranslatableAgent> _pool;

        private Action _dispose = () => { };

        public VisualTranslatorsSpawner(Func<GameObject> poolPreloadFunc, ITranslator translator, string poolContainerName, int preloadCount,
            Transform container, Transform fromTransform, Transform toTransform)
        {
            _translator = translator;
            _poolContainerName = poolContainerName;
            _preloadCount = preloadCount;
            _container = container;
            _fromTransform = fromTransform;
            _toTransform = toTransform;
            InitPool(poolPreloadFunc);
        }

        public TranslatableAgent Spawn()
        {
            TranslatableAgent translatableAgent = _pool.Get();
            _translator.AddTranslatable(translatableAgent.MainTranslatable);
            
            foreach (ITranslatable translatable in translatableAgent.SubTranslatables)
            {
                _translator.AddTranslatable(translatable);
            }
            
            return translatableAgent;
        }

        public void Dispose() =>
            _dispose.Invoke();

        private void InitPool(Func<GameObject> poolPreloadFunc)
        {
            _container = new GameObject(_poolContainerName).transform;
            _pool = new Pool<TranslatableAgent>(
                () =>
                {
                    GameObject poolObject = poolPreloadFunc.Invoke();
                    TranslatableAgent translatableAgent = poolObject.GetComponent<TranslatableAgent>();

                    void OnEndTranslatable(ITranslatable _) => _pool.Return(translatableAgent);

                    translatableAgent.MainTranslatable.End += OnEndTranslatable;
                    _dispose += () => translatableAgent.MainTranslatable.End -= OnEndTranslatable;

                    return translatableAgent;
                },
                GetAction,
                ReturnAction,
                _preloadCount
            );
        }

        private void ReturnAction(TranslatableAgent obj)
        {
            foreach (ITranslatable translatable in obj.SubTranslatables)
            {
                translatable.Disable();
            }
        }

        private void GetAction(TranslatableAgent agent)
        {
            if (agent.MainTranslatable.IsPreload)
            {
                agent.MainTranslatable.Init();
            }
            else
            {
                ITranslatableInit<Vector3> mainTranslatable = (ITranslatableInit<Vector3>) agent.MainTranslatable;
                mainTranslatable.Init(_fromTransform.position, _toTransform.position);
            }

            foreach (ITranslatable translatable in agent.SubTranslatables)
            {
                translatable.Init();
                translatable.Enable();
            }
        }
    }
}