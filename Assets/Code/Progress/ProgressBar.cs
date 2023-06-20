using System;
using Observables;

namespace Progress
{
    public class ProgressBar : IProgressBar
    {
        private const string ReplenishErrorMessage = "Replenish amount must be positive value";
        private const string SpendErrorMessage = "Spend amount must be positive value";
        
        public event Action Full = () => { };
        public event Action Empty = () => { };

        public float Max { get; }
        public Observable<float> Current { get; }
        public float CurrentNormalized => Current.Value / Max;
        public bool IsEmpty => Current.Value <= 0;
        public bool IsFull => Current.Value >= Max;
        public ProgressBar(float max, float current)
        {
            Max = max;
            Current = new Observable<float>(current);
        }

        public void Replenish(float amount)
        {
            if (Validate(amount, ReplenishErrorMessage, () => Current.Value + amount >= Max))
            {
                Current.Value = Max;
                Full.Invoke();
                return;
            }

            Current.Value += amount;
        }

        public void Spend(float amount)
        {
            if (Validate(amount, SpendErrorMessage, () => Current.Value - amount <= 0))
            {
                Current.Value = 0;
                Empty.Invoke();
                return;
            }

            Current.Value -= amount;
        }

        private bool Validate(float amount, string errorMessage, Func<bool> validateFunc)
        {
            if (amount < 0 )
                throw new ArgumentException(errorMessage);

            return validateFunc();
        }
    }
}