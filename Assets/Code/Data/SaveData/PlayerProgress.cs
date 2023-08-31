using System;

namespace Data.SaveData
{
    [Serializable]
    public class PlayerProgress
    {
        public GlobalData GlobalData;
        public LevelData LevelData;

        public PlayerProgress(string initialLevel)
        {
            GlobalData = new GlobalData(initialLevel);
            LevelData = new LevelData(initialLevel);
        }
    }
}