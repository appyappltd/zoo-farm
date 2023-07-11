using UnityEngine;

namespace Logic.Volunteers
{
    [RequireComponent(typeof(TimerOperator))]
    public class VolunteerCreator : MonoBehaviour
    {
        [SerializeField] private float _spawnDelay = 10f;
        [SerializeField] private int _maxCount = 3;
        [SerializeField] private VolunteerBand _volunteerBand;

        private int _activeCount;
        private TimerOperator _timer;

        private void Awake()
        {
            _timer = GetComponent<TimerOperator>();
        }

        private void Start()
        {
            _timer.SetUp(_spawnDelay, OnSpawn);
            OnSpawn();
        }

        private void OnSpawn()
        {
            if (_volunteerBand.VolunteersCount <= _maxCount)
            {
                _volunteerBand.CreateNewVolunteer();
            }

            _timer.Restart();
        }
    }
}
