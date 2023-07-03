using AYellowpaper.SerializedCollections;
using Logic.SpawnPlaces;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Spawn Place Config", fileName = "SpawnPlaceConfig", order = 0)]
    public class SpawnPlaceConfig : ScriptableObject
    {
        public SerializedDictionary<SpawnPlaceId, DefaultSpawnPlace> SpawnPlaces;
    }
}