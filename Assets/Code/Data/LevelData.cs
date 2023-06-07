using System;

namespace Data
{
    [Serializable]
    public class LevelData
    {
        public PlayerLocationData PlayerLocationData;
        
        public LevelData(string levelKey)
        {
            PlayerLocationData = new PlayerLocationData();
        }
    }
}