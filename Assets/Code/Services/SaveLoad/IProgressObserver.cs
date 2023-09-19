using Services.PersistentProgress;
using Services.PersistentProgressGeneric;

namespace Services.SaveLoad
{
    public interface IProgressObserver
    {
        public void Add<TKey>(ISavedProgressReaderGeneric<TKey> observer) where TKey : IProgressKey;
        public void UpdateProgress(IPersistentProgressService progressService);
    }
}