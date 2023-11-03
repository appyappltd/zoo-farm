namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressGeneric<TProgress> : ISavedProgressReaderGeneric<TProgress> where TProgress : IProgressKey
    {
        void UpdateProgress(TProgress progress);
    }
}