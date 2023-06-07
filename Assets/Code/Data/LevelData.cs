using System;
using Services.SaveLoad;

namespace Data
{
    [Serializable]
    public class LevelData : ISaveData
    {
        public PlayerLocationData PlayerLocationData;
        
        public LevelData(string levelKey)
        {
            PlayerLocationData = new PlayerLocationData();
        }
    }
}