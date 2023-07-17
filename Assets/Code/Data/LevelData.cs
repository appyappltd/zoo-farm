using System;

namespace Data
{
    [Serializable]
    public class LevelData
    {
        public string LevelKey;
        public PlayerLocationData PlayerLocationData;

        public LevelData(string levelKey)
        {
            LevelKey = levelKey;
            PlayerLocationData = new PlayerLocationData();
        }
    }
}