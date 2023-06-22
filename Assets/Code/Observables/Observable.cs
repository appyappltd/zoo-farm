using System;
using System.Collections.Generic;

namespace Observables
{
    public class Observable<T> : IObservable<T>
    {
        private T _prev;
        private T _curr;
        private readonly List<ValueDelta<T>> _observers = new();

        public T Value
        {
            get => _curr;
            set => Push(value);
        }

        public Observable()
        {
        }

        public Observable(T value)
        {
            Push(value);
        }

        public void Push(T value)
        {
            _prev = _curr;
            _curr = value;
            Notify();
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Invoke(_prev, _curr);
            }
        }

        public IDisposable Then(ValueDelta<T> observer)
        {
            return new ObservableSubscription<T>(observer, _observers);
        }

        public IDisposable Then(Action<T> observer)
        {
            void Proxy(T prev, T curr) => observer.Invoke(curr);
            return new ObservableSubscription<T>(Proxy, _observers);
        }

        public IDisposable Then(Action observer)
        {
            void Proxy(T prev, T curr) => observer.Invoke();
            return new ObservableSubscription<T>(Proxy, _observers);
        }

        public IDisposable NowAndThen(ValueDelta<T> observer)
        {
            observer.Invoke(_prev, _curr);
            return Then(observer);
        }

        public IDisposable NowAndThen(Action<T> observer)
        {
            observer.Invoke(_curr);
            return Then(observer);
        }

        public IDisposable NowAndThen(Action observer)
        {
            observer.Invoke();
            return Then(observer);
        }
    }
}