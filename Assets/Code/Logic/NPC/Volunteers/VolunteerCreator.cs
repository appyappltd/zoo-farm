using UnityEngine;

namespace Logic.NPC.Volunteers
{
    [RequireComponent(typeof(TimerOperator))]
    public class VolunteerSpawner : MonoBehaviour
    {
        [SerializeField] private float _spawnDelay = 10f;
        [SerializeField] private VolunteerBand _volunteerBand;
        
        private TimerOperator _timer;

        private void Awake()
        {
            _timer = GetComponent<TimerOperator>();
            _timer.SetUp(_spawnDelay, OnSpawn);
        }

        public void Spawn()
        {
            _volunteerBand.CreateNewVolunteer();
        }
        
        public void StartAutoSpawning()
        {
            OnSpawn();
        }
        
        private void OnSpawn()
        {
            if (_volunteerBand.VolunteersCount <= _volunteerBand.MaxVolunteers)
            {
                _volunteerBand.CreateNewVolunteer();
            }

            _timer.Restart();
        }
    }
}
