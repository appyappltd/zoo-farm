using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.AnimalHouses;
using Tools.Comparers;
using UnityEngine;

namespace Services.Animals
{
    public class AnimalsService : IAnimalsService
    {
        private const int AnimalsInPair = 2;
        
        private readonly IAnimalHouseService _houseService;
        private readonly List<IAnimal> _animals = new List<IAnimal>();

        public event Action<IAnimal> Released = _ => { };
        public event Action<IAnimal> Registered = _ => { };

        public int TotalAnimalCount => _animals.Count;
        public int ReleaseReadyAnimalCount => _animals.Count(IsReleaseReady);
        public IReadOnlyList<IAnimal> Animals => _animals;

        public AnimalsService(IAnimalHouseService houseService)
        {
            _houseService = houseService;
        }

        public void Register(IAnimal animal)
        {
            if (_animals.Contains(animal))
                throw new Exception($"Animal {animal} already registered");

            _animals.Add(animal);

#if DEBUG
            Debug.Log($"Animal {animal.AnimalId.Type} (id: {animal.AnimalId.ID}) registered");
#endif
        }

        public void Release(IAnimal animal) =>
            ReleaseAnimal(animal);

        public void Release(AnimalType animalType)
        {
            IAnimal unregisterAnimal = _animals.OrderByDescending(animal => animal.HappinessFactor.Factor).First();
            ReleaseAnimal(unregisterAnimal);
        }

        public IAnimal[] GetAnimals(AnimalType panelAnimalType) =>
            _animals.FindAll(animal => animal.AnimalId.Type == panelAnimalType).ToArray();

        public IEnumerable<AnimalType> GetBreedingReady()
        {
            return _animals.GroupBy(animal => animal.AnimalId.Type).Where(grouping => grouping.Count() > 1)
                .Select(grouping => grouping.Key);
        }

        public IEnumerable<IAnimal> GetReleaseReady() =>
            _animals.Where(IsReleaseReady).Distinct(new AnimalByTypeComparer());

        public BreedingPair SelectPairForBreeding(AnimalType byType)
        {
            IAnimal[] animalsToPair = _animals.OrderBy(animal => animal.HappinessFactor.Factor).Take(AnimalsInPair).ToArray();
            return new BreedingPair(animalsToPair[0], animalsToPair[1]);
        }

        public AnimalCountData GetAnimalsCount(AnimalType panelAnimalType)
        {
            IAnimal[] animals = GetAnimals(panelAnimalType);

            int total = animals.Length;
            int releaseReady = animals.Count(IsReleaseReady);

            return new AnimalCountData(total, releaseReady);
        }

        private static bool IsReleaseReady(IAnimal animal) =>
            animal.Stats.Satiety.IsFull;

        private void ReleaseAnimal(IAnimal releasedAnimal)
        {
            Unregister(releasedAnimal);
            _houseService.VacateHouse(releasedAnimal.AnimalId);
            Released.Invoke(releasedAnimal);

#if DEBUG
            Debug.Log($"Animal {releasedAnimal.AnimalId.Type} (id: {releasedAnimal.AnimalId.ID}) released");
#endif
        }

        private void Unregister(IAnimal animal)
        {
            if (_animals.Contains(animal) == false)
                throw new Exception($"Animal {animal} wasn't registered");

            _animals.Remove(animal);
        }
    }
}