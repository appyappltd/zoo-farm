using System;
using Observables;

namespace Progress
{
    public interface IProgressBarView
    {
        event Action Full;
        event Action Empty;
        float Max { get; }
        Observable<float> Current { get; }
        float CurrentNormalized { get; }
        bool IsEmpty { get; }
        bool IsFull { get; }
    }
}