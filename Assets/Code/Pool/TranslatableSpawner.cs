using Infrastructure.Factory;
using Logic.Translators;
using UnityEngine;

namespace Pool
{
    public class TranslatableSpawner
    {
        private readonly ITranslator _translator;
        private readonly string _poolContainerName;
        private readonly int _preloadCount;
        private readonly Transform _fromTransform;
        private readonly Transform _toTransform;

        private Transform _container;
        private Pool<ITranslatableInit<Vector3>> _pool;

        public TranslatableSpawner(IGameFactory gameFactory, ITranslator translator, string poolContainerName, int preloadCount,
            Transform container, Transform fromTransform, Transform toTransform)
        {
            _translator = translator;
            _poolContainerName = poolContainerName;
            _preloadCount = preloadCount;
            _container = container;
            _fromTransform = fromTransform;
            _toTransform = toTransform;
            InitPool(gameFactory);
        }

        public ITranslatable Spawn()
        {
            ITranslatableInit<Vector3> translatable = _pool.Get();
            translatable.End += TranslatableOnEnd;
            _translator.AddTranslatable(translatable);
            return translatable;
        }

        private void InitPool(IGameFactory gameFactory)
        {
            _container = new GameObject(_poolContainerName).transform;
            _pool = new Pool<ITranslatableInit<Vector3>>(
                () =>
                {
                    GameObject poolObject = gameFactory.CreateVisual(VisualType.Money, Quaternion.identity, _container);
                    return poolObject.GetComponent<ITranslatableInit<Vector3>>();
                },
                GetAction,
                ReturnAction,
                _preloadCount
            );
        }

        private void TranslatableOnEnd(ITranslatable translatable)
        {
            _pool.Return((ITranslatableInit<Vector3>) translatable);
            translatable.End -= TranslatableOnEnd;
        }

        private void ReturnAction(ITranslatable obj)
        {
            obj.Disable();
        }

        private void GetAction(ITranslatableInit<Vector3> obj)
        {
            obj.Init(_fromTransform.position, _toTransform.position);
            obj.Enable();
        }
    }
}