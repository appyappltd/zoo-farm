using Logic.AnimalsBehaviour;

namespace Services.AnimalHouse
{
    public interface IAnimalHouseService : IService
    {
        bool TryTakeHouse(Animal animal);

        void BuildHouse();
    }
}