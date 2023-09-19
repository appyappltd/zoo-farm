namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressReaderGeneric<TProgress>  where TProgress : IProgressKey
    {
        void LoadProgress(in TProgress progress);
    }
}