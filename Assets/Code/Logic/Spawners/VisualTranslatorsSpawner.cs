using System;
using Logic.Translators;
using UnityEngine;

namespace Logic.Spawners
{
    public class VisualTranslatorsSpawner : PooledSpawner<TranslatableAgent>
    {
        private readonly ITranslator _translator;
        private readonly Transform _fromTransform;
        private readonly Transform _toTransform;

        public VisualTranslatorsSpawner(Func<GameObject> poolPreloadFunc,
            int preloadCount, ITranslator translator, Transform fromTransform, Transform toTransform)
            : base(poolPreloadFunc, preloadCount, OnReturnToPool)
        {
            _translator = translator;
            _fromTransform = fromTransform;
            _toTransform = toTransform;
        }

        protected override void OnSpawn(TranslatableAgent spawned)
        {
            _translator.AddTranslatable(spawned.MainTranslatable);

            for (var index = 0; index < spawned.SubTranslatables.Count; index++)
            {
                ITranslatable translatable = spawned.SubTranslatables[index];
                _translator.AddTranslatable(translatable);
            }
        }

        protected override void OnReturn(TranslatableAgent agent)
        {
            for (var index = 0; index < agent.SubTranslatables.Count; index++)
            {
                ITranslatable translatable = agent.SubTranslatables[index];
            }
        }

        protected override void OnGet(TranslatableAgent agent)
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

            for (var index = 0; index < agent.SubTranslatables.Count; index++)
            {
                ITranslatable translatable = agent.SubTranslatables[index];
                translatable.Init();
            }
        }

        private static Action OnReturnToPool(Action returnAction, TranslatableAgent agent)
        {
            void OnEndTranslatable(ITranslatable translatable) => returnAction.Invoke();
            
            agent.MainTranslatable.End += OnEndTranslatable;
            return () => agent.MainTranslatable.End -= OnEndTranslatable;
        }
    }
}