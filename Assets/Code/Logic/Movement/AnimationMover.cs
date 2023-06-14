using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationMover : MonoBehaviour, IMover
{
    public event UnityAction StartMove;
    public event UnityAction GotToPlace;
    [HideInInspector] public bool IsMoving = false;

    [SerializeField, Min(.0f)] private float _offset = 0.1f;

    private Vector3 _targetPosition;

    public void Move(Transform target)
    {
        _targetPosition = target.position;
        StartCoroutine(MoveCor());
    }

    public IEnumerator MoveCor()
    {
        IsMoving = true;
        StartMove?.Invoke();
        while ((transform.position - _targetPosition).magnitude > _offset)
            yield return new WaitForFixedUpdate();
        IsMoving = false;
        GotToPlace?.Invoke();
    }
}
