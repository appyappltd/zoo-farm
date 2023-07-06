using UnityEngine;

namespace Tools
{
    public interface ITransformReadonly
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Scale { get; }
    }
}