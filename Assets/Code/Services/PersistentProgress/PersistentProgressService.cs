using Data.SaveData;
using Infrastructure;

namespace Services.PersistentProgress
{
  public class PersistentProgressService : IPersistentProgressService
  {
    public PlayerProgress Progress { get; set; } = new PlayerProgress(LevelNames.Initial);
  }
}