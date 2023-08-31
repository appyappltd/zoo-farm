using Data.SaveData;

namespace Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get; set; }
  }
}