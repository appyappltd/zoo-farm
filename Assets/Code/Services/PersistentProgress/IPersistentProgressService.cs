using Data;
using Services.SaveLoad;

namespace Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    TData GetData<TData>() where TData : ISaveData;
    PlayerProgress Progress { get; set; }
    void WarmUp();
  }
}