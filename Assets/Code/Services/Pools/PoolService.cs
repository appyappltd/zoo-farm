using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.Pools
{
    public class PoolService
    {
        private readonly Dictionary<Type, IPool<Type>> _allPools = new Dictionary<Type, IPool<Type>>(16);

        public void Spawn<TComponent>() where TComponent : Component
        {
            var pool = _allPools[typeof(TComponent)];
        }
    }
}