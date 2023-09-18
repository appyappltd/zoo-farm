using Tools.PhysicsDebug;
using UnityEngine;

namespace Cameras
{
    public class CameraFrame
    {
        private readonly Plane[] _edgePlanes = new Plane[4];
        private readonly Ray[] _edgeRays = new Ray[4];
        private readonly Camera _camera;

        private readonly Vector3[] _corners =
        {
            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),
        };

#if DEBUG
        private readonly Color[] _colors =
        {
            Color.green,
            Color.red,
            Color.yellow,
            Color.cyan,
        };
#endif
        public CameraFrame(Camera camera) =>
            _camera = camera;

        public Vector3 GetClosestRayPoint(Ray ray, float offset)
        {
            ProjectCross();
            float closestEnter = float.MinValue;
            
            for (int i = 0; i < _edgePlanes.Length; i++)
            {
                _edgePlanes[i].distance -= offset;

                if (_edgePlanes[i].GetSide(ray.origin) == false)
                    continue;

                if (_edgePlanes[i].Raycast(ray, out float enter))
                {
                    if (enter > closestEnter)
                        closestEnter = enter;
                }
                
                _edgePlanes[i].distance += offset;
            }
            
            Vector3 point = ray.GetPoint(closestEnter);
#if DEBUG
            Drawer.DrawSphere(point, 1f, Color.red, 10f);
#endif
            return point;
        }

        private void ProjectCameraRays()
        {
            for (int i = 0; i < _corners.Length; i++)
            {
                _edgeRays[i] = _camera.ViewportPointToRay(_corners[i]);
#if DEBUG
                Debug.DrawRay(_edgeRays[i].origin, _edgeRays[i].direction * 5, _colors[i], 10f);
#endif
            }
        }

        private void ProjectCross()
        {
            ProjectCameraRays();
            
            for (int i = 0; i < _edgePlanes.Length; i++)
            {
                int nextCornerIndex = (i + 1) % _edgePlanes.Length;
                Ray from = _edgeRays[i];
                
                Vector3 to = _edgeRays[nextCornerIndex].origin - _edgeRays[i].origin;
                Vector3 cross = Vector3.Cross(from.direction, to);
                
                _edgePlanes[i] = new Plane(cross, _edgeRays[i].origin);
#if DEBUG
                Debug.DrawRay(_edgeRays[i].origin, _edgePlanes[i].normal * 5, _colors[i], 10f);
#endif
            }
        }
    }
}