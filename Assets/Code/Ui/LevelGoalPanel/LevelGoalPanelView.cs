using System;
using System.Collections.Generic;
using AYellowpaper;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.LevelGoals;
using Logic.TransformGrid;
using Observables;
using Ui.Elements;
using UnityEngine;

namespace Ui.LevelGoalPanel
{
    public class LevelGoalPanelView : MonoBehaviour, IDisposable
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();

        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private TextSetter _rewardText;
        [SerializeField] private BackgroundScaler _backgroundScaler;
        [SerializeField] private Transform _panelsParent;

        public void Construct(IGameFactory gameFactory, GoalConfig config, IGoalProgressView goalProgress)
        {
            InitPanels(gameFactory, config, goalProgress);
            _rewardText.SetText(config.CashRewardForCompletingGoal);
        }

        public void Dispose() =>
            _disposable.Dispose();

        private void InitPanels(IGameFactory gameFactory, GoalConfig config, IGoalProgressView goalProgress)
        {
            foreach (KeyValuePair<AnimalType, int> pair in config.AnimalsToRelease)
            {
                int goalAmount = pair.Value;
                AnimalType animalType = pair.Key;
                
                Transform selfTransform = transform;
                GoalAnimalPanelProvider panel = gameFactory.CreateAnimalGoalPanel(selfTransform.position,
                    selfTransform.rotation, _panelsParent, pair);
                _transformGrid.Value.AddCell(panel.transform);
                _backgroundScaler.AddElement();

                Observables.IObservable<float> progressObservable = goalProgress.GetProgressAmount(animalType);
                progressObservable.Then(releaseProgress =>
                {
                    int releaseAnimalsCount = Mathf.RoundToInt(releaseProgress);
                    panel.CountText.SetText($"{releaseAnimalsCount}/{goalAmount}");
                });
            }
        }
    }
}