using Logic.Animals.AnimalsBehaviour;

namespace Logic.Animals
{
    public struct AnimalPair
    {
        public const int PairCount = 2;
        
        public readonly IAnimal First;
        public readonly IAnimal Second;

        public AnimalPair(IAnimal first, IAnimal second)
        {
            First = first;
            Second = second;
        }
    }
}