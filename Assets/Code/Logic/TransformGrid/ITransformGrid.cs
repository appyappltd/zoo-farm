using UnityEngine;

namespace Logic.TransformGrid
{
    public interface ITransformGrid
    {
        public void AddCell(Transform transform);
        public void RemoveCell(Transform transform);
    }
}