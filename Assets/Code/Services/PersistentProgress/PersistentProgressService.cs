using System;
using System.Collections.Generic;
using Data;
using Services.SaveLoad;
using UnityEngine;

namespace Services.PersistentProgress
{
  public class PersistentProgressService : IPersistentProgressService
  {
    private Dictionary<Type, ISaveData> _saveData;

    public void WarmUp()
    {
      _saveData = new Dictionary<Type, ISaveData>
      {
        [typeof(PlayerLocationData)] = Progress.LevelData.PlayerLocationData
      };
    }
    
    public TData GetData<TData>() where TData : ISaveData
    {
      Debug.Log(typeof(TData));
      _saveData.TryGetValue(typeof(TData), out var data);
      return (TData) data;
    }

    public PlayerProgress Progress { get; set; }
  }
}