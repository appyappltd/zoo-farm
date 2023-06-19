using Logic.Spawners;
using NaughtyAttributes;
using UnityEngine;

namespace Logic
{
    public class TestCoinSpawner : MonoBehaviour
    {
        [SerializeField] private CollectibleCoinSpawner _spawner;
        [SerializeField] private int _coinsToSpawn;

        [Button("Spawn Coins")]
        private void Spawn()
        {
            _spawner.Spawn(_coinsToSpawn);
        }
    }
}