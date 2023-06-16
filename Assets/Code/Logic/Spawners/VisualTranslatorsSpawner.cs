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
            : base(poolPreloadFunc, preloadCount)
        {
            _translator = translator;
            _fromTransform = fromTransform;
            _toTransform = toTransform;

            SetOnReturnToPool((Action));
        }

        private Action Action(Action<TranslatableAgent> arg)
        {
            throw new NotImplementedException();
        }

        protected override void OnSpawn(TranslatableAgent spawned)
        {
            _translator.AddTranslatable(spawned.MainTranslatable);

            foreach (ITranslatable translatable in spawned.SubTranslatables)
            {
                _translator.AddTranslatable(translatable);
            }
        }

        protected override void OnReturn(TranslatableAgent agent)
        {
            foreach (ITranslatable translatable in agent.SubTranslatables)
            {
                translatable.Disable();
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

            foreach (ITranslatable translatable in agent.SubTranslatables)
            {
                translatable.Init();
                translatable.Enable();
            }
        }
    }
}