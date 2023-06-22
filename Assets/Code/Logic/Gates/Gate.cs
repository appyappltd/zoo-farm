using Logic.Translators;
using UnityEngine;

[RequireComponent(typeof(LocalLinearPositionTranslatable))]
public class Gate : MonoBehaviour
{
    [field: SerializeField] public Vector3 ClosePlace { get; private set; }
    [field: SerializeField] public Vector3 OpenPlace { get; private set; }
}
