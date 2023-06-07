using Services.PersistentProgress;

namespace Services.SaveLoad
{
  public interface ISavedProgressGeneric : ISavedProgressReaderGeneric
  {
    void UpdateProgress(IPersistentProgressService progress);
  }
}