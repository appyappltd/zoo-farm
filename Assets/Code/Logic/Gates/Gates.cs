using Logic.Interactions;
using Logic.Translators;
using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(RunTranslator))]
[RequireComponent(typeof(TriggerObserver))]
public class Gates : MonoBehaviour
{
    [SerializeField] private List<Gate> gates = new();

    private RunTranslator translator;

    private void Awake()
    {
        translator = GetComponent<RunTranslator>();

        var trigger = GetComponent<TriggerObserver>();
        trigger.Enter += _ => Open();
        trigger.Exit += _ => Close();
    }

    [Button("Open", enabledMode: EButtonEnableMode.Playmode)]
    private void Open()
    {
        foreach (var gate in gates)
        {
            var positionTranslatable = gate.GetComponent<LocalLinearPositionTranslatable>();
            positionTranslatable.Play(gate.transform.localPosition, gate.OpenPlace);
            translator.AddTranslatable(positionTranslatable);
        }
    }

    [Button("Close", enabledMode: EButtonEnableMode.Playmode)]
    private void Close()
    {
        foreach (var gate in gates)
        {
            var positionTranslatable = gate.GetComponent<LocalLinearPositionTranslatable>();
            positionTranslatable.Play(gate.transform.localPosition, gate.ClosePlace);
            translator.AddTranslatable(positionTranslatable);
        }
    }
}
