using System;

namespace Logic.Observable
{
    public interface IObservable<out T>
    {
        event Action<T> Changed;
    }
}