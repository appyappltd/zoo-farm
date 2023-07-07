using System;
using Logic.Spawners;
using UnityEngine;

namespace Logic.Plants
{
    public class GardenBedObserver : MonoBehaviour
    {
        [SerializeField] private GardenBed _gardenBed;
        [SerializeField] private HandItemSpawner _handItemSpawner;
        
        private void Awake()
        {
            _gardenBed.GrowUp += OnGrowUp;
        }

        private void OnDestroy()
        {
            _gardenBed.GrowUp -= OnGrowUp;
        }

        private void OnGrowUp()
        {
            _handItemSpawner.Spawn();
        }
    }
}