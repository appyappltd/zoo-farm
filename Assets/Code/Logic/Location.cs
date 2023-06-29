using UnityEngine;

namespace Logic
{
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