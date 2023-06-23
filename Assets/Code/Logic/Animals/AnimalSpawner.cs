using Data.ItemsData;
using UnityEngine;

namespace Logic.Animals
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField] private HandItem _animal;

        public HandItem InstantiateAnimal(Transform parent) => Instantiate(_animal, parent);
    }
}
