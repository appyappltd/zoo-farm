namespace Progress
{
    public interface IProgressBar : IProgressBarView
    {
        void Spend(float newSpeed);
        void Replenish(float amount);
        void Reset();
        void SetMinNonZero();
        void SetMaxNonFull();
    }
}