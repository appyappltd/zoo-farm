namespace Progress
{
    public interface IProgressBar : IProgressBarView
    {
        void Spend(float amount);
        void Replenish(float amount);
        void Reset();
        void SetMinNonZero();
        void SetMaxNonFull();
    }
}