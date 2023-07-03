using StaticData;
using UnityEngine;

namespace Logic.SpawnPlaces
{
    public class SpawnPlace : MonoBehaviour
    {
        [SerializeField] private DefaultSpawnPlace _spawnPlace;

        private void Awake()
        {
            _spawnPlace.SetUp(transform);
        }
    }
}