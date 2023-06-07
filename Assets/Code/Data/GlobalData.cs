using System;

namespace Data
{
    [Serializable]
    public class GlobalData
    {
        public Settings Settings;
        
        public GlobalData()
        {
            Settings = new Settings();
        }
    }
}