using UnityEngine;

public class VolunteerStateMachine : MonoBehaviour
{
    public BaseState CurrentState { get; set; }

    [SerializeField] private BaseState _initState;

    private void Awake()
    {
        CurrentState = _initState;
    }
    private void Update()
    {
        CurrentState.Execute(this);
    }
}
