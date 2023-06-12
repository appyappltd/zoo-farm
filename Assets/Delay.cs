using Logic.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Delay : MonoBehaviour
{
    public event UnityAction<GameObject> Complete;

    [SerializeField, Min(.0f)] private float _delay = 1f;

    private void Awake()
    {
        var trigger = GetComponent<TriggerObserver>();
        trigger.Enter += obj => StartCoroutine(Wait(obj));
        trigger.Exit += obj => StopCoroutine(Wait(obj));
    }

    private IEnumerator Wait(GameObject obj)
    {
        yield return new WaitForSeconds(_delay);
        Complete?.Invoke(obj);
    }
}
