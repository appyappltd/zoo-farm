using System;
using System.Collections.Generic;
using Services.PersistentProgressGeneric;

namespace Services.SaveLoad
{
    public class ProgressObserver<TKey> where TKey : IProgressKey
    {
        private const string RemoveException = "You are trying to remove a non-existent element";
        
        private readonly HashSet<ISavedProgressReaderGeneric<TKey>> _readers;
        private readonly HashSet<ISavedProgressGeneric<TKey>> _updaters;

        public IEnumerator<ISavedProgressReaderGeneric<TKey>> Readers => _readers.GetEnumerator();
        public IEnumerator<ISavedProgressGeneric<TKey>> Updaters => _updaters.GetEnumerator();

        public ProgressObserver(ISavedProgressReaderGeneric<TKey> reader)
        {
            _readers = new HashSet<ISavedProgressReaderGeneric<TKey>>();
            _updaters = new HashSet<ISavedProgressGeneric<TKey>>();
            
            Add(reader);
        }

        public void Add(ISavedProgressReaderGeneric<TKey> observer)
        {
            _readers.Add(observer);

            if (observer is ISavedProgressGeneric<TKey> updater)
                _updaters.Add(updater);
        }

        public void Remove(ISavedProgressReaderGeneric<TKey> observer)
        {
            if (_readers.Contains(observer) == false)
                throw new ArgumentOutOfRangeException(nameof(observer), RemoveException);

            _readers.Remove(observer);

            if (observer is ISavedProgressGeneric<TKey> updater)
                _updaters.Remove(updater);
        }
    }
}