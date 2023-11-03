using System;
using Services.PersistentProgressGeneric;

namespace Data.SaveData
{
    [Serializable]
    public class Settings: IProgressKey
    {
        public string SettingName;
    }
}