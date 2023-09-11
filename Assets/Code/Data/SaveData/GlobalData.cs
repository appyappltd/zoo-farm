using System;
using Services.PersistentProgressGeneric;

namespace Data.SaveData
{
    [Serializable]
    public class GlobalData : IProgressKey
    {
        public Settings Settings;
        public String LastLevel;
        
        public GlobalData(string initialLevel)
        {
            LastLevel = initialLevel;
            Settings = new Settings();
        }
    }
}