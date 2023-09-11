namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressReaderGeneric<in TProgress>
    {
        void LoadProgress(TProgress progress);
    }
}