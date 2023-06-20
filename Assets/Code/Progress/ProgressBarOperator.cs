using System;

namespace Progress
{
    public class ProgressBarOperator
    {
        private readonly Action<float> _onUpdate;

        public ProgressBarOperator(IProgressBar bar, float changingSpeed, bool isNegativeSpeed)
        {
            if (changingSpeed <= 0)
                throw new ArgumentException("Changing speed must be greater than zero");

            if (isNegativeSpeed)
            {
                _onUpdate = (deltaTime) => bar.Spend(changingSpeed * deltaTime * bar.Max);
            }
            else
            {
                _onUpdate = (deltaTime) => bar.Replenish(changingSpeed * deltaTime * bar.Max);
            }
        }

        public void Update(float deltaTime) =>
            _onUpdate.Invoke(deltaTime);
    }
}