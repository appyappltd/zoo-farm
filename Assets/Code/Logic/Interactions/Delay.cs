using System;
using Logic.Interactions;
using System.Collections;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

[RequireComponent(typeof(TriggerObserver))]
public class Delay : MonoBehaviour
{
    public event Action<GameObject> Complete = c => { };

    [SerializeField, Min(.0f)] private float _delay = 1f;

    [SerializeField] private float time = .0f;
    private Coroutine coroutine;

    private void Awake()
    {
        var trigger = GetComponent<TriggerObserver>();
        trigger.Enter += obj =>
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(Wait(obj));
        };
        trigger.Exit += _ =>
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(Reverse());
        };
    }

    private IEnumerator Wait(GameObject obj)
    {
        while (_delay > time)
        {
            var addTime = Time.deltaTime;
            time += addTime;
            yield return new WaitForSeconds(addTime);
        }
        Complete.Invoke(obj);
    }

    private IEnumerator Reverse()
    {
        while (time > 0) 
        {
            var addTime = Time.deltaTime;
            time -= addTime;
            yield return new WaitForSeconds(addTime);
        }
    }
}
