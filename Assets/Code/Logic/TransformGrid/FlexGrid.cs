using UnityEngine;

namespace Logic.TransformGrid
{
    public class FlexGrid : ITransformGrid
    {
        public void AddCell(Transform transform)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveCell(Transform transform)
        {
            throw new System.NotImplementedException();
        }
    }

    public interface ITransformGrid
    {
        public void AddCell(Transform transform);
        public void RemoveCell(Transform transform);
    }
}