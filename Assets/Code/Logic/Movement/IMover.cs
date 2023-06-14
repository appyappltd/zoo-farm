using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IMover
{
    public event UnityAction StartMove;
    public event UnityAction GotToPlace;

    public void Move(Transform target);
    public IEnumerator MoveCor();
}
