using Services.PersistentProgress;

namespace Services.SaveLoad
{
    public interface ISavedProgressReaderGeneric
    {
        void LoadProgress(IPersistentProgressService progress);
    }
}