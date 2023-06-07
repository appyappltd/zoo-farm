using System;

namespace Data
{
    [Serializable]
    public class PlayerProgress
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