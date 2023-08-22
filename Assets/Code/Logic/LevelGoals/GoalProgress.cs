using System;
using System.Collections.Generic;
using Logic.Animals;
using Progress;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class GoalProgress
    {
        private readonly Dictionary<AnimalType, ProgressBar> _releaseProgressBars;
        
        private int _noTFullBarCount;

        public event Action Compleated = () => { };
        
        public List<Observables.IObservable<float>> Observables { get; }

        public GoalProgress(GoalPreset goalPreset)
        {
            int capacity = goalPreset.AmountAnimalToRelease.Count;
            _releaseProgressBars = new Dictionary<AnimalType, ProgressBar>(capacity);
            Observables = new List<Observables.IObservable<float>>(capacity);
            
            foreach (var (animalType, goalAmount) in goalPreset.AmountAnimalToRelease)
                RegisterProgressBar(animalType, goalAmount);
        }

        public void AddToReleased(AnimalType withType)
        {
            if (_releaseProgressBars.TryGetValue(withType, out ProgressBar bar))
                bar.Increment();
            else
                throw new NullReferenceException(nameof(withType));

            foreach (KeyValuePair<AnimalType, ProgressBar> progressBar in _releaseProgressBars)
            {
                Debug.Log($"Progress {progressBar.Key}, {progressBar.Value}");
            }
        }

        private void RegisterProgressBar(AnimalType animalType, int goalAmount)
        {
            ProgressBar progressBar = new ProgressBar(goalAmount);
            Observables.Add(progressBar.Current);
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
            _noTFullBarCount--;
            
            if (_noTFullBarCount <= 0)
                Compleated.Invoke();
        }
    }
}