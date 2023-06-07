using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// ����� ��� ����. �����, �� ���������, ���� �� ����� ����, ��� ����� ��������� �� �������.
public class VolunteerSpawner : MonoBehaviour
{
    public event UnityAction<VolunteerMovement> SpawnV;

    [SerializeField, Min(.0f)] private float _time = 10f;
    [SerializeField] private Transform _spawnPlace;

    [SerializeField] private VolunteerMovement _volunteer;

    // �� ����, ��� ������ ��� ������ ���������� � ����-����. ����� ���� ����� ���.

    private void Awake()
    {
        StartCoroutine(CreateVolunteer());
    }

    private IEnumerator CreateVolunteer()
    {
        yield return new WaitForSeconds(3);

        while (true)
        {
            var v = Instantiate(_volunteer, _spawnPlace.position,
                                            _spawnPlace.rotation);
            SpawnV?.Invoke(v);
            yield return new WaitForSeconds(_time);
        }
    }
}
