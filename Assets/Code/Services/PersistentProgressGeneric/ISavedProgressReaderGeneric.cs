namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressReaderGeneric<TProgress> : IProgressReaderRegistrable where TProgress : IProgressKey
    {
        void LoadProgress(in TProgress progress);
    }

    public interface IProgressReaderRegistrable
    {
    }
}