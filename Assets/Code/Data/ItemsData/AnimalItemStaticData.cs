using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/Animal Item Data", fileName = "NewAnimalItemData", order = 0)]
    public class AnimalItemStaticData : ScriptableObject
    {
        [field: SerializeField] public ItemId ItemId { get; set; }
        [field: SerializeField] public int Weight { get; set; }
        public AnimalType AnimalType;
        public BeginStats BeginStats;
    }
}