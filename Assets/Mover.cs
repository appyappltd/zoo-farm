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

    public void MoveTowards(Transform target)
    {
        this.target = target;
        _targetPosition = target.position;
        StartCoroutine(MoveTowardsCor());
    }

    public void MoveAnimation(Transform target)
    {
        this.target = target;
        _targetPosition = target.position;
        StartCoroutine(MoveAnimationCor());
    }

    private IEnumerator MoveAnimationCor()
    {
        IsMoving = true;
        StartMove?.Invoke();
        while ((transform.position - _targetPosition).magnitude > _offset)
            yield return new WaitForFixedUpdate();
        IsMoving = false;
        GotToPlace?.Invoke();
    }

    private IEnumerator MoveTowardsCor()
    {
        IsMoving = true;
        StartMove?.Invoke();
        while ((transform.position - target.position).magnitude > _offset
              && lastPos != transform.position)
        {
            lastPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         _speed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        IsMoving = false;
        GotToPlace?.Invoke();
    }
}
