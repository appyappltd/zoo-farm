using UnityEngine;

namespace Services.Input
{
    public interface IInputReader
    {
        public Vector2 Direction { get; }
        float Horizontal { get; }
        float Vertical { get; }
        void Init();
    }
}