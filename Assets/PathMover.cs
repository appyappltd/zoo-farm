using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMover : MonoBehaviour
{
    [SerializeField] private Transform[] _points;

    private int index = 0;
    private Mover mover;
    private Rotater rotater;
    private bool IsPath = false;

    private void Awake()
    {
        rotater = GetComponent<Rotater>();
        mover = GetComponent<Mover>();
        mover.GotToPlace += SetPoint;
    }

    public void StartWalk()
    {
        IsPath = true;
        Walk();
    }

    public void SetPoints(Transform[] points) => _points = points;

    public void Walk()
    {
        var point = _points[index];
        rotater.Rotate(point);
        mover.MoveAnimation(point);
    }

    private void SetPoint()
    {
        if (IsPath && index + 1 < _points.Length)
        {
            index++;
            Walk();
        }
    }
}
