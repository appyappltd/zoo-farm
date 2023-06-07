using System;
using Services.SaveLoad;

namespace Data
{
    [Serializable]
    public class PlayerProgress : ISaveData
    {
        public GlobalData GlobalData;
        public LevelData LevelData;

        public PlayerProgress(string initialLevel)
        {
            GlobalData = new GlobalData();
            LevelData = new LevelData(initialLevel);
        }
    }
}