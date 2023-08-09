using Logic.Animals.AnimalsBehaviour;

namespace Services.Animals
{
    public struct BreedingPair
    {
        public readonly IAnimal First;
        public readonly IAnimal Second;

        public BreedingPair(IAnimal first, IAnimal second)
        {
            First = first;
            Second = second;
        }
    }
}