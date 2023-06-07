using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    public void Rotate(Transform target)
    {
        //var vectorToTarget = target.position - transform.position;
        //var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0,angle + 40,0);
        var vectorToTarget = target.position - transform.position;
        var angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(-angle, Vector3.up);
    }
}
