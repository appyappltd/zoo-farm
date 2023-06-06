using UnityEngine;

namespace Services.Input
{
    public interface IPlayerInputService : IService
    {
        void Cleanup();
        void RegisterInputReader(IInputReader inputReader);
        Vector2 Direction { get; }
        float Horizontal { get; }
        float Vertical { get; }
    }
}