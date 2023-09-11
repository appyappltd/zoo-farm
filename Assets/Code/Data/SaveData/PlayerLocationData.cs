using System;
using Services.PersistentProgressGeneric;

namespace Data.SaveData
{
    [Serializable]
    public class PlayerLocationData : IProgressKey
    {
        public Vector3Data Position = new Vector3Data(0,0,0);
        public Vector3Data Rotation = new Vector3Data(0,0,0);
    }
}