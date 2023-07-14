using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Medicine;

namespace Data.ItemsData
{
    public class AnimalItemData : IItemData
    {
        private readonly AnimalItemStaticData _data;
        private readonly MedicineToolId _treatToolId;

        public ItemId ItemId => _data.ItemId;
        public int Weight => _data.Weight;
        public AnimalType Type => _data.AnimalType;
        public MedicineToolId TreatToolId => _treatToolId;
        public BeginStats BeginStats => _data.BeginStats;
        
        public AnimalItemData(AnimalItemStaticData data, MedicineToolId treatToolId)
        {
            _data = data;
            _treatToolId = treatToolId;
        }
    }
}