using System.Collections.Generic;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.LevelGoals;
using Observables;
using UnityEngine;

namespace Ui.LevelGoalPanel
{
    public class LevelGoalPanelView : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        public void Construct(IGameFactory gameFactory, GoalPreset preset, IGoalProgressView goalProgress)
        {
            InitPanels(gameFactory, preset, goalProgress);
            //TODO: заполнение награды за уровень
        }

        private void InitPanels(IGameFactory gameFactory, GoalPreset preset, IGoalProgressView goalProgress)
        {
            foreach (KeyValuePair<AnimalType, int> pair in preset.AnimalsToRelease)
            {
                int goalAmount = pair.Value;
                AnimalType animalType = pair.Key;
                
                Transform selfTransform = transform;
                GoalAnimalPanelProvider panel = gameFactory.CreateAnimalGoalPanel(selfTransform.position,
                    selfTransform.rotation, selfTransform, pair);

                IObservable<float> progressObservable = goalProgress.GetProgressAmount(animalType);
                
                progressObservable.Then(releaseProgress =>
                {
                    int releaseAnimalsCount = Mathf.RoundToInt(releaseProgress);
                    panel.CountText.SetText($"{releaseAnimalsCount}/{goalAmount}");
                });
            }
        }
    }
}