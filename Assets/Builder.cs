using Logic.Interactions;
using Logic.Translators;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TriggerObserver))]
[RequireComponent(typeof(Delay))]
[RequireComponent(typeof(RunTranslator))]
public class Builder : MonoBehaviour
{
    [SerializeField] private List<GameObject> _components;
    [SerializeField] private Vector2 _offset;

    private List<Vector3> defSizes = new();
    private List<Vector3> defPositions = new();
    private RunTranslator translator;
    private bool isBuild = false;

    private void Awake()
    {
        translator = GetComponent<RunTranslator>();

        foreach (var c in _components)
        {
            defSizes.Add(c.transform.localScale);
            defPositions.Add(c.transform.position);

            c.transform.localScale = Vector3.zero;
            c.transform.position += new Vector3(Random.Range(_offset.x, _offset.y),
                                                Random.Range(0, _offset.y),
                                                Random.Range(_offset.x, _offset.y));

            GetComponent<Delay>().Complete += _ => Build();
        }
    }

    private void Build()
    {
        if (isBuild)
            return;

        for (int i = 0; i < _components.Count; i++)
        {
            var position = _components[i].GetComponent<LinearPositionTranslatable>();
            var scale = _components[i].GetComponent<LinearScaleTranslatable>();

            position.Init(_components[i].transform.position, defPositions[i]);
            scale.Init(_components[i].transform.localScale, defSizes[i]);

            translator.AddTranslatable(position);
            translator.AddTranslatable(scale);
        }
        isBuild = true;
    }
}
