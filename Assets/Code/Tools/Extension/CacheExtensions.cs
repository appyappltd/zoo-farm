using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools.Extension
{
    public class CacheExtensions
    {
        public static TValue GetCashed<TKey, TValue>(IDictionary<TKey, TValue> collection, TKey key, string path)
            where TValue : Object
        {
            if (collection.TryGetValue(key, out TValue value))
                return value;

            TValue loaded = Resources.Load<TValue>($"{path}/{key}");

            if (loaded is null)
                throw new NullReferenceException($"There is no object with key {key}");

            collection.Add(key, loaded);
            return loaded;
        }
    }
}