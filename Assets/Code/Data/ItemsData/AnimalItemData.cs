using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Medicine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/Animal Item Data", fileName = "NewAnimalItemData", order = 0)]
    public class AnimalItemData : ScriptableObject, IItemData
    {
        [field: SerializeField] public ItemId ItemId { get; set; }
        [field: SerializeField] public int Weight { get; set; }
        public AnimalId AnimalId;
        public MedicineToolId TreatToolId;
        public BeginStats BeginStats;

        private void Awake()
        {
            AnimalId = new AnimalId(AnimalType.CatB, GetInstanceID());
        }
    }
}