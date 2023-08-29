using Logic.Animals.AnimalsBehaviour;

namespace Services.Breeding
{
    public interface IAnimalBreedService : IService
    {
        void Register(IAnimal animal);
        void Unregister(IAnimal animal);
    }
}