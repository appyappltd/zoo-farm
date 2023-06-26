using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour;

namespace Services.Animals
{
    public interface IAnimalsService : IService
    {
        IReadOnlyList<IAnimal> Animals { get; }
        void Register(IAnimal animal);
        void Release(IAnimal animal);
    }
}