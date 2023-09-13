using System;

namespace Observables
{
    public class EventDisposer : IDisposable
    {
        private readonly Action _unsubscribe;

        public EventDisposer(Action unsubscribe) =>
            _unsubscribe = unsubscribe;

        public void Dispose() =>
            _unsubscribe.Invoke();
    }
}