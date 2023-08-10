using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Medical;

namespace Data.ItemsData
{
    public class AnimalItemData : IItemData
    {
        private readonly AnimalItemStaticData _data;
        private readonly MedicalToolId _treatToolId;

        public ItemId ItemId => StaticData.ItemId;
        public int Weight => StaticData.Weight;
        public AnimalType Type => StaticData.AnimalType;
        public MedicalToolId TreatToolId => _treatToolId;
        public BeginStats BeginStats => StaticData.BeginStats;
        public AnimalItemStaticData StaticData => _data;

        public AnimalItemData(AnimalItemStaticData data, MedicalToolId treatToolId)
        {
            _data = data;
            _treatToolId = treatToolId;
        }
    }
}