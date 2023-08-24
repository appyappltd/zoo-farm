using Infrastructure.Factory;
using Logic.LevelGoals;
using UnityEngine;

namespace Ui
{
    public class LevelGoalPanelView : MonoBehaviour
    {
        public void Construct(IGameFactory gameFactory, GoalPreset preset, IGoalProgressView goalProgress)
        {
            
        }

        private void InitPanels(IGameFactory gameFactory, GoalPreset preset)
        {
            foreach (var pair in preset.AnimalsToRelease)
            {
                Transform selfTransform = transform;
                gameFactory.CreateAnimalGoalPanel(selfTransform.position, selfTransform.rotation, selfTransform, pair);
            }
        }
    }
}