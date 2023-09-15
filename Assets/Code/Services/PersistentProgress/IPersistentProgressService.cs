using System;
using Data.SaveData;
using Services.PersistentProgressGeneric;

namespace Services.PersistentProgress
{
  public interface IPersistentProgressService : IService
  {
    PlayerProgress Progress { get;}
    void Init(PlayerProgress progress);
    TProgress GetProgress<TProgress>() where TProgress : IProgressKey;
    IProgressKey GetProgress(Type byType);
  }
}