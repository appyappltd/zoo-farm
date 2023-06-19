using System;
using Logic.Translators;
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

        protected override void OnGet(TranslatableAgent agent)
        {
            if (agent.MainTranslatable.IsPreload)
            {
                agent.MainTranslatable.Play();
            }
            else
            {
                ITranslatableParametric<Vector3> mainTranslatable = (ITranslatableParametric<Vector3>) agent.MainTranslatable;
                mainTranslatable.Play(_fromTransform.position + _fromPositionOffset, _toTransform.position);
            }

            for (var index = 0; index < agent.SubTranslatables.Count; index++)
            {
                ITranslatable translatable = agent.SubTranslatables[index];
                translatable.Play();
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