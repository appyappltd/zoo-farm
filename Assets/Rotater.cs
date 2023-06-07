using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    private Rigidbody rb;

    private void Awake() => rb = GetComponent<Rigidbody>();

    public void Rotate(Transform target)
    {
        // rb.MoveRotation(transform.LookAt(target);

        //var lookRotaton = Quaternion.LookRotation(target.position);
        //rb.MoveRotation(lookRotaton);
        var vectorToTarget = target.position - transform.position;
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);
    }
}
