namespace Progress
{
    public interface IProgressBarView
    {
        float Max { get; }
        float Current { get; }
        float CurrentNormalized { get; }
        bool IsEmpty { get; }
    }
}