using System;

namespace Progress
{
    public interface IProgressBarView
    {
        event Action Full;
        event Action Empty;
        float Max { get; }
        float Current { get; }
        float CurrentNormalized { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
    }
}