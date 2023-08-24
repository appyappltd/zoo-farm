using System;
using System.Collections.Generic;
using Logic.Animals;
using Progress;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class GoalProgress : IGoalProgressView
    {
        private readonly Dictionary<AnimalType, ProgressBar> _releaseProgressBars;
        
        private int _notFullBarCount;

        public event Action Compleated = () => { };

        public GoalProgress(GoalPreset goalPreset)
        {
            int capacity = goalPreset.AnimalsToRelease.Count;
            _releaseProgressBars = new Dictionary<AnimalType, ProgressBar>(capacity);

            foreach (var (animalType, goalAmount) in goalPreset.AnimalsToRelease)
                RegisterProgressBar(animalType, goalAmount);
        }

        public void AddToReleased(AnimalType withType)
        {
            if (_releaseProgressBars.TryGetValue(withType, out ProgressBar bar))
                bar.Increment();
            else
                throw new NullReferenceException(nameof(withType));

#if DEBUG
            foreach (KeyValuePair<AnimalType, ProgressBar> progressBar in _releaseProgressBars)
            {
                Debug.Log($"Progress {progressBar.Key}, {progressBar.Value}");
            }
#endif
        }

        public Observables.IObservable<float> GetProgressAmount(AnimalType byType)
        {
            if (_releaseProgressBars.TryGetValue(byType, out ProgressBar bar))
                return bar.Current;

            throw new ArgumentNullException(nameof(byType));
        }

        private void RegisterProgressBar(AnimalType animalType, int goalAmount)
        {
            ProgressBar progressBar = new ProgressBar(goalAmount);
            _releaseProgressBars.Add(animalType, progressBar);

            if (progressBar.IsFull)
                return;

            void OnFull()
            {
                CheckForGoal();
                progressBar.Full -= OnFull;
            }
            
            progressBar.Full += OnFull;
        }

        private void CheckForGoal()
        {
            _notFullBarCount--;
            
            if (_notFullBarCount <= 0)
                Compleated.Invoke();
        }
    }
}