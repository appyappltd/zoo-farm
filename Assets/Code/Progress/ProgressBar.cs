using System;

namespace Progress
{
    public class ProgressBar : IProgressBar
    {
        private const string ReplenishErrorMessage = "Replenish amount must be positive value";
        private const string SpendErrorMessage = "Spend amount must be positive value";

        public event Action Full = () => { };
        public event Action Empty = () => { };

        public float Max { get; }
        public float Current { get; private set; }
        public float CurrentNormalized => Current / Max;
        public bool IsEmpty => Current <= 0;
        public bool IsFull => Current >= Max;
        public ProgressBar(float max, float current)
        {
            Max = max;
            Current = current;
        }

        public void Replenish(float amount)
        {
            if (Validate(amount, ReplenishErrorMessage, () => Current + amount > Max))
            {
                Current = Max;
                Full.Invoke();
                return;
            }

            Current += amount;
        }

        public void Spend(float amount)
        {
            if (Validate(amount, SpendErrorMessage, () => Current - amount < 0))
            {
                Current = 0;
                Empty.Invoke();
                return;
            }

            Current -= amount;
        }

        private bool Validate(float amount, string errorMessage, Func<bool> validateFunc)
        {
            if (amount < 0 )
                throw new ArgumentException(errorMessage);

            return validateFunc();
        }
    }
}