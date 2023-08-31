using Data.SaveData;

namespace Services.PersistentProgress
{
  public interface ISavedProgressReader
  {
    void LoadProgress(PlayerProgress progress);
  }
}