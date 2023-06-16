using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Tools.Extension
{
    public static class RandomExtension
    {
        public static TElement GetRandom<TElement>(this IEnumerable<TElement> elements)
        {
            var elementsArray = elements.ToArray();
            int randomIndex = Random.Range(0, elementsArray.Length);
            return elementsArray[randomIndex];
        }

        public static Vector3 GetRandomAroundPosition(this Vector3 position, Vector3 maxOffset)
        {
            return position + new Vector3(
                Random.Range(-maxOffset.x, maxOffset.x),
                Random.Range(-maxOffset.y, maxOffset.y),
                Random.Range(-maxOffset.z, maxOffset.z));
        }
    }
}