using System;
using AYellowpaper;
using Data;
using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Player;
using Logic.TransformGrid;
using Services;
using Services.Animals;
using Services.StaticData;
using UnityEngine;

namespace Logic.Houses
{
    public class HouseFoundationView : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private HeroInteraction _heroInteraction;
        
        private HouseFoundation _foundation;

        private void Awake()
        {
            Construct(
                AllServices.Container.Single<IStaticDataService>(),
                AllServices.Container.Single<IGameFactory>(),
                AllServices.Container.Single<IAnimalsService>().AnimalCounter);
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void Construct(IStaticDataService staticData, IGameFactory gameFactory, IAnimalCounter animalCounter)
        {
            _foundation = new HouseFoundation(staticData, gameFactory, _transformGrid.Value, animalCounter);

            Subscribe();
        }

        private void Subscribe()
        {
            _heroInteraction.Interacted += OnInteracted;
            _heroInteraction.Canceled += OnCanceled;
        }

        private void Unsubscribe()
        {
            _heroInteraction.Interacted -= OnInteracted;
            _heroInteraction.Canceled -= OnCanceled;
        }

        private void OnInteracted(Hero _) =>
            _foundation.ShowBuildChoice();

        private void OnCanceled() =>
            _foundation.HideBuildChoice();
    }
}