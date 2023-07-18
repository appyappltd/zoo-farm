using UnityEngine;

namespace Services.Pools
{
    public interface IPoolable
    {
        GameObject GameObject { get; }
    }
}