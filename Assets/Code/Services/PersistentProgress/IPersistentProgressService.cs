using Data.SaveData;

namespace Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get;}
    void Init(PlayerProgress progress);
  }
}