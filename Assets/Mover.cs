using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mover : MonoBehaviour
{
    public event UnityAction StartMove;
    public event UnityAction GotToPlace;
    public bool IsMoving = false;

    [SerializeField, Min(.0f)] private float _speed = 5.0f;
    [SerializeField, Min(.0f)] private float _offset = 0.1f;

    private Transform target = null;
    private Vector3 lastPos = Vector3.zero;

    public void Move(Transform target, bool useMoveTowards = true)
    {
        this.target = target;
        StartCoroutine(MoveCor(useMoveTowards));
    }

    private IEnumerator MoveCor(bool useMoveTowards = true)
    {
        StartMove?.Invoke();
        IsMoving = true;
        while ((transform.position - target.position).magnitude > _offset
                && (!useMoveTowards || lastPos != transform.position))
        {
            lastPos = transform.position;
            if (useMoveTowards)
                transform.position = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         _speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        IsMoving = false;
        GotToPlace?.Invoke();
    }
}
