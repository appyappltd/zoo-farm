using UnityEngine;

namespace Services.Input
{
    public interface IInputReader
    {
        public Vector2 Direction { get; }
        void Init();
    }
}