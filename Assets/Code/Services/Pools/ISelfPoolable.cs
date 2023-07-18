using System;

namespace Services.Pools
{
    public interface ISelfPoolable : IPoolable
    {
        event Action<ISelfPoolable> Disabled;
    }
}