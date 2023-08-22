using AYellowpaper;
using Infrastructure.Factory;
using Logic.TransformGrid;
using Services;
using Services.Animals;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class LevelGoalView : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ITransformGrid, MonoBehaviour> _transformGrid;
        [SerializeField] private GoalPreset _temporalGoalPreset;

        private LevelGoal _goal;
        private ReleaseInteractionsGrid _releaseInteractions;

        private void Awake()
        {
            Construct(AllServices.Container.Single<IGameFactory>(),
                AllServices.Container.Single<IAnimalsService>());
        }

        private void OnDestroy()
        {
            _releaseInteractions.Dispose();
            _goal.Dispose();
        }

        public void Construct(IGameFactory gameFactory, IAnimalsService animalsService)
        {
            _releaseInteractions = new ReleaseInteractionsGrid(gameFactory, _transformGrid.Value, _temporalGoalPreset.AmountAnimalToRelease.Keys);
            _goal = new LevelGoal(animalsService, _releaseInteractions, _temporalGoalPreset);
        }
    }
}