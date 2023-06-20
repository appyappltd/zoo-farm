using Cameras;
using UnityEngine;

namespace Services.Camera
{
    public interface ICameraOperatorService : IService
    {
        void Focus(Transform followTarget);
        void Focus(Vector3 onPosition);
        void RegisterCamera(CameraMovement cameraMovement);
        void FocusOnDefault();
        void SetAsDefault(Transform target);
    }
}