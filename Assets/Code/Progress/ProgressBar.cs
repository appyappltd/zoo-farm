using System;

namespace Progress
{
    public class ProgressBar : IProgressBar
    {
        public float Max { get; }
        public float Current { get; private set; }
        public float CurrentNormalized { get; }
        public float ReduceSpeed { get; private set; }
        public bool IsEmpty => Current <= 0;
        public ProgressBar(float max, float current)
        {
            Max = max;
            Current = current;
        }

        public void Tick(float deltaTime)
        {
            if (IsEmpty)
                return;

            Current -= deltaTime * ReduceSpeed;

            if (Current <= 0)
                Current = 0;
        }

        public void Replenish(float amount)
        {
            if (Validate(amount,  () => Current + amount < Max))
                return;

            Current += amount;
        }

        private bool Validate(float amount, Func<bool> func)
        {
            throw new NotImplementedException();
        }

        private bool Validate(float amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Replenish amount must be positive value");

            if (Current + amount > Max)
            {
                Current = Max;
                return true;
            }

            return false;
        }

        public void Spend(float amount)
        {
            if (amount <= 0 )
                throw new ArgumentException("Replenish amount must be positive value");

            if (Current - amount < 0)
            {
                Current = 0;
                return;
            }

            Current -= amount;
        }
    }
}