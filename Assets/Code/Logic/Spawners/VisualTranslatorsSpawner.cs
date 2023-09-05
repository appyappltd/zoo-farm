using System;
using Logic.Translators;
using Services.Pools;
using UnityEngine;

namespace Logic.Spawners
{
    public class VisualTranslatorsSpawner : PooledSpawner<TranslatableAgent>
    {
        private readonly Vector3 _fromPositionOffset = new Vector3(0, 1, 0); 
        
        private readonly ITranslator _translator;
        private readonly Transform _fromTransform;
        private readonly Transform _toTransform;

        public VisualTranslatorsSpawner(Func<GameObject> poolPreloadFunc,
            int preloadCount, ITranslator translator, Transform fromTransform, Transform toTransform, IPoolService poolService)
            : base(poolPreloadFunc, preloadCount, OnReturnToPool, poolService)
        {
            _translator = translator;
            _fromTransform = fromTransform;
            _toTransform = toTransform;
        }

        protected override void OnSpawn(TranslatableAgent spawned)
        {
            _translator.AddTranslatable(spawned.Main);

            if (spawned.Main.IsPreload)
            {
                spawned.Main.Play();
            }
            else
            {
                ITranslatableParametric<Vector3> mainTranslatable = (ITranslatableParametric<Vector3>) spawned.Main;
                mainTranslatable.Play(_fromTransform.position + _fromPositionOffset, _toTransform.position);
            }

            for (var index = 0; index < spawned.Sub.Count; index++)
            {
                ITranslatable translatable = spawned.Sub[index];
                _translator.AddTranslatable(translatable);
                translatable.Play();
            }
        }

        private static Action OnReturnToPool(Action returnAction, TranslatableAgent agent)
        {
            void OnEndTranslatable(ITranslatable translatable) => returnAction.Invoke();
            
            agent.Main.End += OnEndTranslatable;
            return () => agent.Main.End -= OnEndTranslatable;
        }
    }
}