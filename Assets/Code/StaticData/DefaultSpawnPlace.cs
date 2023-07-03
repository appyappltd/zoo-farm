using Logic.SpawnPlaces;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Default Spawn Position", fileName = "NewDefaultSpawnPosition", order = 0)]
    public class DefaultSpawnPlace : ScriptableObject
    {
        public SpawnPlaceId SpawnPlaceId;

        private Transform _defaultSpawnPlace;
        
        public Vector3 SpawnPositionByDefault => _defaultSpawnPlace.position;
        public Transform SpawnPlaceByDefault => _defaultSpawnPlace;

        public void SetUp(Transform transform)
        {
            _defaultSpawnPlace = transform;
        }
    }
}