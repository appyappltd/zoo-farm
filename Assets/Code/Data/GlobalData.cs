using System;
using Services.SaveLoad;

namespace Data
{
    [Serializable]
    public class GlobalData : ISaveData
    {
        public Settings Settings;
        
        public GlobalData()
        {
            Settings = new Settings();
        }
    }
}