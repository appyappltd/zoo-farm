namespace Progress
{
    public interface IProgressBar : IProgressBarView
    {
        void SetReduceSpeed(float newSpeed);
        void Replenish(float amount);
    }
}