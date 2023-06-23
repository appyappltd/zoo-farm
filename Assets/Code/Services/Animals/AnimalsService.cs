using System;
using System.Collections.Generic;
using Logic.AnimalsBehaviour;

namespace Services.Animals
{
    public class AnimalsService
    {
        private readonly List<Animal> _animals = new List<Animal>();
        
        public void Register(Animal animal)
        {
            if (_animals.Contains(animal))
                throw new Exception($"Animal {animal} already registered");
            
            _animals.Add(animal);
        }

        public void Unregistered(Animal animal)
        {
            if (_animals.Contains(animal) == false)
                throw new Exception($"Animal {animal} wasn't registered");

            _animals.Remove(animal);
        }
    }
}