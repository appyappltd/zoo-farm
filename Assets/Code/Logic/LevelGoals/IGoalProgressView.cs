using System;
using Logic.Animals;

namespace Logic.LevelGoals
{
    public interface IGoalProgressView
    {
        event Action Compleated;
        Observables.IObservable<float> GetProgressAmount(AnimalType byType);
    }
}