using System.Diagnostics;
using NaughtyAttributes;
using Services;
using Services.Camera;
using UnityEngine;

namespace Tools
{
    public class CameraFrameTest : MonoBehaviour
    {
        private ICameraOperatorService _camera;

        private void Awake()
        {
            _camera = AllServices.Container.Single<ICameraOperatorService>();
        }

        private void Update()
        {
            Cast();
        }

        [Button]
        [Conditional("UNITY_EDITOR")]
        private void Cast()
        {
            _camera.GetClosestRayPoint(new Ray(transform.position, transform.forward), 0f);
        }
    }
}