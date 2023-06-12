using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Mover : MonoBehaviour
{
    public event UnityAction StartMove;
    public event UnityAction GotToPlace;
    [HideInInspector] public bool IsMoving = false;

    [SerializeField, Min(.0f)] private float _speed = 5.0f;
    [SerializeField, Min(.0f)] private float _offset = 0.1f;

    private Vector3 _targetPosition;
    private Transform target = null;
    private Vector3 lastPos = Vector3.zero;


    public void Move(Transform target, bool useMoveTowards = true, bool useTarget = true)
    {
        this.target = target;
        _targetPosition = target.position;
        StartCoroutine(MoveCor(useMoveTowards, useTarget));
    }

    private IEnumerator MoveCor(bool useMoveTowards, bool useTarget)
    {
        IsMoving = true;
        StartMove?.Invoke();
        while (((useTarget &&
              (transform.position - target.position).magnitude > _offset)
           || (transform.position - _targetPosition).magnitude > _offset)
           && (!useMoveTowards || lastPos != transform.position))
        {
            if (useMoveTowards)
            {
                lastPos = transform.position;
                transform.position = Vector3.MoveTowards(transform.position,
                                                             target.position,
                                                             _speed * Time.fixedDeltaTime);
            }
            yield return new WaitForFixedUpdate();
        }
        IsMoving = false;
        GotToPlace?.Invoke();
    }
}
