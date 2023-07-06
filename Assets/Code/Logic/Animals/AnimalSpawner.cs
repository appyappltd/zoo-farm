using Data.ItemsData;
using UnityEngine;

namespace Logic.Animals
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField] private HandItem _animal;
        [SerializeField] private AnimalItemData _preloadAnimalData;

        public HandItem InstantiateAnimal(Vector3 at)
        {
            HandItem instantiateAnimal = Instantiate(_animal, at, Quaternion.identity);
            instantiateAnimal.Construct(_preloadAnimalData);
            return instantiateAnimal;
        }
    }
}
