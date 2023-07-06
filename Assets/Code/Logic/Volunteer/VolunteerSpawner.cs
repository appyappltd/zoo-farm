using UnityEngine;

namespace Logic.Volunteer
{
    public class VolunteerSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private Volunteer _volunteer;

        public Volunteer Spawn()
        {
            return Instantiate(_volunteer, _spawnPlace.position, _spawnPlace.rotation);
        }
    }
}
