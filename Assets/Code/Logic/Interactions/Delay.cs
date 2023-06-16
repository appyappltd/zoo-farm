using System;
using Logic.Interactions;
using System.Collections;
using UnityEngine;

public class Delay : MonoBehaviour
{
    public event Action<GameObject> Complete = c => { };

    [SerializeField, Min(.0f)] private float _delay = 1f;

    private void Awake()
    {
        var trigger = GetComponent<TriggerObserver>();
        trigger.Enter += obj => StartCoroutine(Wait(obj));
        trigger.Exit += _ => StopAllCoroutines();
    }

    private IEnumerator Wait(GameObject obj)
    {
        yield return new WaitForSeconds(_delay);
        Complete.Invoke(obj);
    }
}
