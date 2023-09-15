namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressGeneric<TProgress> : ISavedProgressReaderGeneric<TProgress>
    {
        void UpdateProgress(ref TProgress progress);
    }
}