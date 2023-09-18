using UnityEngine;

namespace Tools.PhysicsDebug
{
    public static class Drawer
    {
        public static void DrawRay(Vector3 from, Vector3 to, Color color, float lifetime = 1f)
        {
            Debug.DrawRay(from, to, color, lifetime);
        }

        public static void DrawRay(Vector3 from, Vector3 direction, float length, Color color, float lifetime = 1f)
        {
            Debug.DrawRay(from, direction.normalized * length, color, lifetime);
        }

        public static void DrawCube(Vector3 origin, float size, Color color, float lifeTime = 1f)
        {
            Vector3 leftCorner = origin - Vector3.one * size / 2f;
            Debug.DrawRay(leftCorner, Vector3.up, color, lifeTime);
            Debug.DrawRay(leftCorner, Vector3.forward, color, lifeTime);
            Debug.DrawRay(leftCorner, Vector3.right, color, lifeTime);
            
            leftCorner = origin + Vector3.one * size / 2f;
            Debug.DrawRay(leftCorner, -Vector3.up, color, lifeTime);
            Debug.DrawRay(leftCorner, -Vector3.forward, color, lifeTime);
            Debug.DrawRay(leftCorner, -Vector3.right, color, lifeTime);
        }
        
        public static void DrawSphere(Vector3 origin, float size, Color color, float lifeTime = 1f)
        {
            Debug.DrawRay(origin, Vector3.up * size, color, lifeTime);
            Debug.DrawRay(origin, Vector3.down * size, color, lifeTime);
            Debug.DrawRay(origin, Vector3.left * size, color, lifeTime);
            Debug.DrawRay(origin, Vector3.right * size, color, lifeTime);
            Debug.DrawRay(origin, Vector3.forward * size, color, lifeTime);
            Debug.DrawRay(origin, Vector3.back * size, color, lifeTime);
        }
    }
}