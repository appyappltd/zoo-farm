using NTC.Global.Cache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoCache
{
    [SerializeField] private Transform target;
    [SerializeField] private float distance = 20;
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float _smoothing;

    private Vector3 offset;

    private void Start() =>
        offset = transform.rotation * Vector3.back * distance;

    protected override void FixedRun()
    {
        Vector3 targetCamPos = (target.position + offset + cameraOffset);
        transform.position = Vector3.Lerp(transform.position,
                                          targetCamPos,
                                          _smoothing * Time.fixedDeltaTime);
    }
}
