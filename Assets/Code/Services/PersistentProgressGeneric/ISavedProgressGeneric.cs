namespace Services.PersistentProgressGeneric
{
    public interface ISavedProgressGeneric<TProgress> : IProgressRegistrable, ISavedProgressReaderGeneric<TProgress> where TProgress : IProgressKey
    {
        void UpdateProgress(TProgress progress);
    }
    
    public interface IProgressRegistrable
    {
    }
}