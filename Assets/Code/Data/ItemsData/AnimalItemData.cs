using Logic.Animals;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(fileName = "Animal Item Data", menuName = "animalItemData")]
    public class AnimalItemData : ItemData
    {
        [field: SerializeField] public AnimalType AnimalType { get; private set; }
    }
}