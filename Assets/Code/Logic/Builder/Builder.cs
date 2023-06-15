using Logic.Interactions;
using Logic.Translators;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerObserver))]
[RequireComponent(typeof(Delay))]
[RequireComponent(typeof(RunTranslator))]
public class Builder : MonoBehaviour
{
    [SerializeField] private List<GameObject> _components;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private GameObject _sine;
    [SerializeField, Min(.0f)] private float _time = .1f;

    private List<Vector3> defSizes = new();
    private List<Vector3> defPositions = new();
    private RunTranslator translator;
    private Delay delay;

    private bool isBuild = false;

    private void Awake()
    {
        translator = GetComponent<RunTranslator>();
        delay = GetComponent<Delay>();

        delay.Complete += _ => StartBuild();

        foreach (var c in _components)
        {
            defSizes.Add(c.transform.localScale);
            defPositions.Add(c.transform.position);

            c.transform.localScale = Vector3.zero;
            c.transform.position += new Vector3(Random.Range(_offset.x, _offset.y),
                                                Random.Range(0, _offset.y),
                                                Random.Range(_offset.x, _offset.y));
        }
    }

    [Button("Build", enabledMode: EButtonEnableMode.Playmode)]
    private void StartBuild()
    {
        if (isBuild)
            return;
        if (_sine)
            Destroy(_sine.gameObject);

        isBuild = true;
        StartCoroutine(Build());

        delay.Complete -= _ => Build();
    }

    private IEnumerator Build()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            var position = _components[i].GetComponent<LinearPositionTranslatable>();
            var scale = _components[i].GetComponent<LinearScaleTranslatable>();

            position.Init(_components[i].transform.position, defPositions[i]);
            scale.Init(_components[i].transform.localScale, defSizes[i]);

            translator.AddTranslatable(position);
            translator.AddTranslatable(scale);
            yield return new WaitForSeconds(_time);
        }
    }
}
