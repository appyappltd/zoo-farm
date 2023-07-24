using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour;

namespace Tools.Comparers
{
    public class AnimalByTypeComparer : IEqualityComparer<IAnimal>
    {
        public bool Equals(IAnimal x, IAnimal y)
        {
        
            if (ReferenceEquals(x, y))
                return true;
        
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                return false;
        
            return x.AnimalId.Type == y.AnimalId.Type;
        }

        public int GetHashCode(IAnimal animal) =>
            animal.AnimalId.Type.GetHashCode();
    }
}