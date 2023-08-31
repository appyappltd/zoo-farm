using Data.SaveData;

namespace Services.SaveLoad
{
  public interface ISaveLoadService : IService
  {
    void SaveProgress();
    bool LoadProgress(out GlobalData globalData, out LevelData levelData);
  }
}