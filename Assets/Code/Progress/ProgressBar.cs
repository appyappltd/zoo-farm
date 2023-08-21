using System;
using Observables;

namespace Progress
{
    public class ProgressBar : IProgressBar
    {
        private const string ReplenishErrorMessage = "Replenish amount must be positive value";
        private const string SpendErrorMessage = "Spend amount must be positive value";
        
        private const float SingleUnit = 1f;
        private const float ZeroUnit = 0f;
        private const float MinEpsilonFactor = 0.001f;

        public event Action Full = () => { };
        public event Action Empty = () => { };

        public float Max { get; }
        public Observable<float> Current { get; }
        public float CurrentNormalized => Current.Value / Max;
        public bool IsEmpty => Current.Value <= float.Epsilon;
        public bool IsFull => Current.Value >= Max - float.Epsilon;

        public ProgressBar(float max, float current = 0f)
        {
            Max = max;
            Current = new Observable<float>(current);
        }

        public void Reset() =>
            Current.Value = ZeroUnit;

        public void SetMinNonZero()
        {
            Current.Value = Max * MinEpsilonFactor;
        }

        public void SetMaxNonFull()
        {
            Current.Value = Max - Max * MinEpsilonFactor;
        }

        public void Increment()
        {
            Replenish(SingleUnit);
        }

        public void Decrement()
        {
            Spend(SingleUnit);
        }
        
        public void Replenish(float amount)
        {
            if (IsFull)
                return;

            if (Validate(amount, ReplenishErrorMessage, () => Current.Value + amount >= Max - float.Epsilon))
            {
                Current.Value = Max;
                Full.Invoke();
                return;
            }

            Current.Value += amount;
        }

        public void Spend(float amount)
        {
            if (IsEmpty)
                return;

            if (Validate(amount, SpendErrorMessage, () => Current.Value - amount <= float.Epsilon))
            {
                Current.Value = 0;
                Empty.Invoke();
                return;
            }

            Current.Value -= amount;
        }

        private bool Validate(float amount, string errorMessage, Func<bool> validateFunc)
        {
            if (amount < 0)
                throw new ArgumentException(errorMessage);

            return validateFunc();
        }
    }
}