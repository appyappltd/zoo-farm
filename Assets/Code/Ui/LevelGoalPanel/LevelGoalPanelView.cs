using System;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper;
using Data;
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
        private readonly Dictionary<AnimalType, GoalAnimalPanelProvider> _panels = new Dictionary<AnimalType, GoalAnimalPanelProvider>();

        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private TextSetter _rewardText;
        [SerializeField] private BackgroundScaler _backgroundScaler;
        [SerializeField] private Transform _panelsParent;

        private IGameFactory _gameFactory;
        private LevelGoal _levelGoal;

        public void Construct(IGameFactory gameFactory, LevelGoal levelGoal)
        {
            _levelGoal = levelGoal;
            _gameFactory = gameFactory;
            
            levelGoal.Updated += UpdateView;
            InitView();
        }

        public void Dispose()
        {
            _disposable.Dispose();
            _backgroundScaler.Reset();
        }

        private void UpdateView()
        {
            Dispose();
            InitView();
        }

        private void InitView()
        {
            SingleGoalData singleGoalData = _levelGoal.GetCurrentGoal();
            InitPanels(singleGoalData, _levelGoal.Progress);
            _rewardText.SetText(singleGoalData.CashRewardForCompletingGoal);
        }

        private void InitPanels(SingleGoalData goalData, IGoalProgressView goalProgress)
        {
            foreach (KeyValuePair<AnimalType, int> pair in goalData.AnimalsToRelease)
            {
                int goalAmount = pair.Value;
                AnimalType animalType = pair.Key;

                GoalAnimalPanelProvider panel = GetPanel(animalType, pair);
                
                _backgroundScaler.AddElement();
                UpdateText(0, panel, goalAmount);

                Observables.IObservable<float> progressObservable = goalProgress.GetProgressAmount(animalType);
                _disposable.Add(progressObservable.Then(releaseProgress =>
                {
                    UpdateText(releaseProgress, panel, goalAmount);
                }));
            }

            foreach (var unusedPanel in _panels.Keys.Except(goalData.AnimalsToRelease.Keys)) 
                _transformGrid.Value.RemoveCell(_panels[unusedPanel].transform);
        }

        private GoalAnimalPanelProvider GetPanel(AnimalType animalType, KeyValuePair<AnimalType, int> pair)
        {
            Transform selfTransform = transform;

            if (_panels.TryGetValue(animalType, out GoalAnimalPanelProvider panel))
                return panel;
            
            panel = _gameFactory
                .CreateAnimalGoalPanel(selfTransform.position, selfTransform.rotation, _panelsParent, pair);
            _panels.Add(animalType, panel);
            
            _transformGrid.Value.AddCell(panel.transform);

            return panel;
        }

        private void UpdateText(float releaseProgress, GoalAnimalPanelProvider panel, int goalAmount)
        {
            int releaseAnimalsCount = Mathf.RoundToInt(releaseProgress);
            panel.CountText.SetText($"{releaseAnimalsCount}/{goalAmount}");
        }
    }
}