using System;
using NaughtyAttributes;
using Services;
using Services.AnimalHouse;
using UnityEngine;

namespace Logic
{
    public class TestHouseBuild : MonoBehaviour
    {
        [SerializeField] private Vector3 _spawnPlace;

        private void Awake()
        {
            Build();
        }

        [Button("Build")]
        private void Build()
        {
            AllServices.Container.Single<IAnimalHouseService>().BuildHouse(_spawnPlace);
        }
    }
}