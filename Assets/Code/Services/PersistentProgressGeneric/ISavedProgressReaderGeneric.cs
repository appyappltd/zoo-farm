namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressReaderGeneric<TProgress>
    {
        void LoadProgress(in TProgress progress);
    }
}