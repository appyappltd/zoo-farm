using UnityEngine;

namespace Logic.Movement
{
    public class Rotater : MonoBehaviour
    {
        public void Rotate(Transform target)
        {
            //var vectorToTarget = target.position - transform.position;
            //var angle = Mathf.Atan2(vectorToTarget.z, vectorToTarget.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.Euler(0, angle, 0);
            transform.rotation = Quaternion.Euler(0,0,0);
            var vectorToTarget = target.position - transform.position;
            var angle = Mathf.Atan2(vectorToTarget.z, vectorToTarget.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
        }
    }
}
