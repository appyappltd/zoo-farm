namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressGeneric<in TProgress> : ISavedProgressReaderGeneric<TProgress>
    {
        void UpdateProgress(TProgress progress);
    }
}