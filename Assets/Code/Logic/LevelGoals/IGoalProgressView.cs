using System;
using System.Collections.Generic;

namespace Logic.LevelGoals
{
    public interface IGoalProgressView
    {
        event Action Compleated;
        List<Observables.IObservable<float>> Observables { get; }
    }
}