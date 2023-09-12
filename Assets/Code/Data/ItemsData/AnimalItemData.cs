using Logic.Animals;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Medical;

namespace Data.ItemsData
{
    public class AnimalItemData : IItemData
    {
        private readonly AnimalItemStaticData _data;
        private readonly TreatToolId _treatToolId;

        public ItemId ItemId => _data.ItemId;
        public int Weight => _data.Weight;
        public AnimalType Type => _data.AnimalType;
        public TreatToolId TreatToolId => _treatToolId;
        public BeginStats BeginStats => _data.BeginStats;
        public AnimalItemStaticData StaticData => _data;

        public AnimalItemData(AnimalItemStaticData data, TreatToolId treatToolId)
        {
            _data = data;
            _treatToolId = treatToolId;
        }
    }
}