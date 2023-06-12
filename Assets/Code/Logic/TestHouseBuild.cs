using NaughtyAttributes;
using Services;
using Services.AnimalHouse;
using UnityEngine;

namespace Logic
{
    public class TestHouseBuild : MonoBehaviour
    {
        [Button("Build")]
        private void Build()
        {
            AllServices.Container.Single<IAnimalHouseService>().BuildHouse();
        }
    }
}