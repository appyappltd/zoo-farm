using Data.SaveData;
using Logic.Player;
using Services.PersistentProgressGeneric;

namespace Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    bool LoadProgress(out GlobalData globalData, out LevelData levelData);
    void Register<TProgress>(IProgressReaderRegistrable reader) where TProgress : IProgressKey;
  }
}