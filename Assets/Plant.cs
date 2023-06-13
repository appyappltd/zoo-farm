using Data.ItemsData;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Delay))]
public class Plant : MonoBehaviour
{
    public event UnityAction<GameObject> GrowUp;

    [SerializeField] private List<GameObject> _stages;
    [SerializeField, Min(.0f)] private float _time;

    private bool canGrow = true;

    private void Awake()
       => GetComponent<Delay>().Complete += _ =>
       {
           if (canGrow)
               StartCoroutine(Grow());
       };

    private IEnumerator Grow()
    {
        canGrow = false;
        yield return new WaitForSeconds(_time);

        var currStage = Instantiate(_stages[0], transform);
        for (int i = 1; i < _stages.Count; i++)
        {
            yield return new WaitForSeconds(_time);
            Destroy(currStage);
            currStage = Instantiate(_stages[i], transform);
        }
        GrowUp?.Invoke(currStage);

        var drop = currStage.GetComponent<DropItem>();
        drop.PickUp += _ => canGrow = true;
    }
}
