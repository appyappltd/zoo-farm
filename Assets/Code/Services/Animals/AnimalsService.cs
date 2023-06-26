using System;
using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour;
using Services.AnimalHouses;
using UnityEngine;

namespace Services.Animals
{
    public class AnimalsService : IAnimalsService
    {
        private readonly IAnimalHouseService _houseService;
        private readonly List<IAnimal> _animals = new List<IAnimal>();

        public AnimalsService(IAnimalHouseService houseService)
        {
            _houseService = houseService;
        }

        public IReadOnlyList<IAnimal> Animals => _animals;

        public void Register(IAnimal animal)
        {
            if (_animals.Contains(animal))
                throw new Exception($"Animal {animal} already registered");
            
            _animals.Add(animal);

            Debug.Log($"Animal {animal.AnimalId.Type} (id: {animal.AnimalId.ID}) registered");
        }

        public void Release(IAnimal animal)
        {
            Unregister(animal);
            _houseService.VacateHouse(animal.AnimalId);
            animal.Destroy();
            
            Debug.Log($"Animal {animal.AnimalId.Type} (id: {animal.AnimalId.ID}) released");
        }

        private void Unregister(IAnimal animal)
        {
            if (_animals.Contains(animal) == false)
                throw new Exception($"Animal {animal} wasn't registered");

            _animals.Remove(animal);
        }
    }
}