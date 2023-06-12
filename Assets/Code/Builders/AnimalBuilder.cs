using Logic.AnimalsBehaviour;

namespace Builders
{
    public class AnimalBuilder
    {
        public void Build(Animal animal)
        {
            AnimalId animalId = new AnimalId(AnimalType.CatB, animal.GetHashCode());
            animal.Construct(animalId);
        }
    }
}