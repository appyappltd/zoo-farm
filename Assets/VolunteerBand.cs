using System.Collections.Generic;
using Tools.Extension;
using UnityEngine;

public class VolunteerBand : MonoBehaviour
{
    [SerializeField, Min(.0f)] private float _distance = 1.5f;
    [SerializeField] private Transform _defTarget, _target;
    [SerializeField] private VolunteerSpawner _spawner;

    private List<VolunteerMovement> volunteers = new();

    private void Awake()
    {
        UpdateTarget();
        _spawner.SpawnV += AddVolunteers;
    }

    public void AddVolunteers(VolunteerMovement volunteer)
    {
        volunteers.Add(volunteer);

        var mover = volunteer.GetComponent<Mover>();
        var rotater = volunteer.GetComponent<Rotater>();

        mover.Move(_target, false);
        rotater.Rotate(_target);
        mover.GotToPlace += () => rotater.Rotate(_defTarget);

        UpdateTarget();
    }

    private void UpdateTarget()
     => _target.position = _target.position.ChangeX(_target.position.x + _distance);
}
