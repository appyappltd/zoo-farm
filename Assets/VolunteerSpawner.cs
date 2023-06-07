using System.Collections;
using UnityEngine;
using UnityEngine.Events;

//  ласс так себе. ƒумаю, он временный, пока не будет €сно, что будет творитьс€ на уровн€х.
public class VolunteerSpawner : MonoBehaviour
{
    public event UnityAction<VolunteerMovement> SpawnV;

    [SerializeField, Min(.0f)] private Vector2 _time = new(5,40);
    [SerializeField] private Transform _spawnPlace;

    [SerializeField] private VolunteerMovement _volunteer;

    // Ќе знаю, как именно они должны по€вл€тьс€ и тыры-пыры. ѕусть пока будет так.

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
            yield return new WaitForSeconds(Random.Range(5,40));
        }
    }
}
