using System;
using System.Collections.Generic;

namespace Data.SaveData
{
    [Serializable]
    public class LevelData
    {
        public string LevelKey;
        public PlayerLocationData PlayerLocationData;
        public List<AnimalData> _animalDatas;

        public LevelData(string levelKey)
        {
            LevelKey = levelKey;
            PlayerLocationData = new PlayerLocationData();
            _animalDatas = new List<AnimalData>();
        }
    }
}