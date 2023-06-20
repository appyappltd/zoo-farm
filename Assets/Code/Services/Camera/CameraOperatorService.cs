using Cameras;
using UnityEngine;

namespace Services.Camera
{
    public class CameraOperatorService : ICameraOperatorService
    {
        private CameraMovement _activeCamera;
        
        public void Focus(Transform followTarget)
        {
            _activeCamera.Follow(followTarget);
        }

        public void Focus(Vector3 onPosition)
        {
            _activeCamera.MoveTo(onPosition);
        }
        
        public void RegisterCamera(CameraMovement cameraMovement)
        {
            _activeCamera = cameraMovement;
        }
    }
}