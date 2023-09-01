using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Player;
using Logic.TransformGrid;
using Observables;
using Services.PersistentProgress;
using Services.StaticData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic.CellBuilding.Foundations
{
    public abstract class Foundation<TEnum> : IFoundation where TEnum : Enum
    {
        protected readonly IStaticDataService StaticData;
        protected readonly IPersistentProgressService PersistentProgress;
        protected readonly IGameFactory GameFactory;
        
        private readonly ITransformGrid _transformGrid;
        protected readonly Transform SelfTransform;
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        private readonly Dictionary<TEnum, ChoseInteractionProvider> _choseInteractions =
            new Dictionary<TEnum, ChoseInteractionProvider>();

        protected Foundation(IStaticDataService staticData, IPersistentProgressService persistentProgress, IGameFactory gameFactory, Transform selfTransform, ITransformGrid transformGrid)
        {
            StaticData = staticData;
            PersistentProgress = persistentProgress;
            GameFactory = gameFactory;
            SelfTransform = selfTransform;
            _transformGrid = transformGrid;

            CreateAllInteractions();
        }

        protected abstract IReadOnlyCollection<TEnum> GetAvailableTypes();
        protected abstract IReadOnlyCollection<TEnum> GetAllPossibleTypes();
        protected abstract void CreateBuilding(ChoseInteractionProvider choseZone, TEnum associatedType);
        protected abstract ChoseInteractionProvider CreateChoseZone(TEnum withType);

        public void Dispose() =>
            _disposable?.Dispose();

        public void ShowBuildChoice()
        {
            foreach (TEnum animalType in GetAvailableTypes())
            {
                _transformGrid.AddCell(_choseInteractions[animalType].transform);
            }
        }

        public void HideBuildChoice() =>
            _transformGrid.RemoveAll();

        private void CreateAllInteractions()
        {
            foreach (TEnum availableType in GetAllPossibleTypes())
            {
                ChoseInteractionProvider choseZone = CreateChoseZone(availableType);
                _choseInteractions.Add(availableType, choseZone);

                Subscribe(choseZone, availableType);
            }
        }
        
        private void Subscribe(ChoseInteractionProvider choseZone, TEnum associatedType)
        {
            void OnChosen(Hero hero)
            {
                CreateBuilding(choseZone, associatedType);

                Object.Destroy(SelfTransform.gameObject);
            }

            choseZone.Interaction.Interacted += OnChosen;
            _disposable.Add(new EventDisposer(() => choseZone.Interaction.Interacted -= OnChosen));
        }
    }
}