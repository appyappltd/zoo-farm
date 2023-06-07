using System;
using Services.SaveLoad;

namespace Data
{
    [Serializable]
    public class PlayerLocationData : ISaveData
    {
        public Vector3Data Position = new Vector3Data(0,0,0);
        public Vector3Data Rotation = new Vector3Data(0,0,0);
    }
}