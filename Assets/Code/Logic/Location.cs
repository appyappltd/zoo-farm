using System;
using UnityEngine;

namespace Logic
{
    [Serializable]
    public struct Location
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public Location(Vector3 position, Quaternion rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}