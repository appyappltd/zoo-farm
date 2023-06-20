using System;

namespace Observables
{
    public interface IObservable<T>
    {
        T Value { get; }
        IDisposable Then(ValueDelta<T> observer);
        IDisposable Then(Action<T> observer);
        IDisposable Then(Action observer);
        IDisposable NowAndThen(ValueDelta<T> observer);
        IDisposable NowAndThen(Action<T> observer);
        IDisposable NowAndThen(Action observer);
    }
}