using UnityEngine;

namespace Logic.TransformGrid
{
    public interface ITransformGrid
    {
        public void AddCell(Transform cellTransform);
        public void RemoveCell(Transform cellTransform);
    }
}