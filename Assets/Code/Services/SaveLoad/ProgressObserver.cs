using System;
using System.Collections.Generic;
using Services.PersistentProgress;
using Services.PersistentProgressGeneric;

namespace Services.SaveLoad
{
    public class ProgressObserver<TKey> : IProgressObserver where TKey : IProgressKey
    {
        private const string RemoveException = "You are trying to remove a non-existent element";
        
        private readonly HashSet<ISavedProgressReaderGeneric<TKey>> _readers;
        private readonly HashSet<ISavedProgressGeneric<TKey>> _updaters;

        public ProgressObserver(ISavedProgressReaderGeneric<TKey> reader)
        {
            _readers = new HashSet<ISavedProgressReaderGeneric<TKey>>();
            _updaters = new HashSet<ISavedProgressGeneric<TKey>>();
            
            Add(reader);
        }
        
        public void Remove(ISavedProgressReaderGeneric<TKey> observer)
        {
            if (_readers.Contains(observer) == false)
                throw new ArgumentOutOfRangeException(nameof(observer), RemoveException);

            _readers.Remove(observer);

            if (observer is ISavedProgressGeneric<TKey> updater)
                _updaters.Remove(updater);
        }
        
        public void Add<TPKey>(ISavedProgressReaderGeneric<TPKey> observer) where TPKey : IProgressKey
        {
            if (observer is ISavedProgressReaderGeneric<TKey> reader)
            {
                _readers.Add(reader);

                if (observer is ISavedProgressGeneric<TKey> updater)
                    _updaters.Add(updater);
            }
        }

        public void UpdateProgress(IPersistentProgressService progressService)
        {
            CleanNulls();

            foreach (ISavedProgressGeneric<TKey> updater in _updaters)
                updater.UpdateProgress(progressService.GetProgress<TKey>());
        }

        private void CleanNulls()
        {
            _readers.RemoveWhere(match => match.Equals(null));
            _updaters.RemoveWhere(match => match.Equals(null));
        }
    }
}