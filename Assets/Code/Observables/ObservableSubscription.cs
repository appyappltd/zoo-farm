using System;
using System.Collections.Generic;

namespace Observables
{
    public class ObservableSubscription<T> : IDisposable
    {
        private readonly ValueDelta<T> _observer;
        private readonly List<ValueDelta<T>> _observers;

        public ObservableSubscription(ValueDelta<T> observer, List<ValueDelta<T>> observers)
        {
            _observer = observer;
            _observers = observers;
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _observers.Add(_observer);
        }

        private void Unsubscribe()
        {
            _observers.Remove(_observer);
        }
    }
}