using UnityEngine;

public class Gate : MonoBehaviour
{
    [field: SerializeField] public Vector3 ClosePlace { get; private set; }
    [field: SerializeField] public Vector3 OpenPlace { get; private set; }

    private void Awake()
    {
        ClosePlace += transform.position;
        OpenPlace += transform.position;
    }
}
