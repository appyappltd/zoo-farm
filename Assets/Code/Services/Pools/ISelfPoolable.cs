using System;
using UnityEngine;

namespace Services.Pools
{
    public interface ISelfPoolable : IPoolable
    {
        event Action<ISelfPoolable> Disabled;
    }
}