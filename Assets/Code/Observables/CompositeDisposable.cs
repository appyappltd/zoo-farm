using System;
using System.Collections.Generic;

namespace Observables
{
    public class CompositeDisposable : IDisposable
    {
        private readonly Stack<IDisposable> _disposables = new();

        public void Add(IDisposable disposable)
        {
            _disposables.Push(disposable);
        }

        public void Dispose()
        {
            while (_disposables.TryPop(out var disposable))
            {
                disposable.Dispose();
            }
        }
    }
}