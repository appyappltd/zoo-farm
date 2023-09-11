using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.AnimalCounter;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services.AnimalHouses;
using Services.Breeding;
using Tools;
using Tools.Comparers;
using Tools.Global;
using UnityEngine;

namespace Services.Animals
{
    public class AnimalsService : IAnimalsService
    {
        private const int AnimalsInPair = 2;
        
        private readonly IAnimalHouseService _houseService;

        private readonly List<IAnimal> _animals = new List<IAnimal>();
        private readonly AnimalCounter _animalCounter = new AnimalCounter();

        private AnimalBreedService _breedService;
        
        public event Action<IAnimal> Released = _ => { };
        public event Action<IAnimal> Registered = _ => { };

        public int TotalAnimalCount => _animals.Count;
        public int ReleaseReadyAnimalCount => _animals.Count(IsReleaseReady);
        public IReadOnlyList<IAnimal> Animals => _animals;
        public IAnimalCounter AnimalCounter => _animalCounter;

        public AnimalsService(IAnimalHouseService houseService)
        {
            _houseService = houseService;
        }

        public void Register(IAnimal animal)
        {
            if (_animals.Contains(animal))
                throw new Exception($"Animal {animal} already registered");

            _animals.Add(animal);
            _animalCounter.Register(animal);
            Registered.Invoke(animal);
#if DEBUG
            Debug.Log($"Animal {animal.AnimalId.Type} (id: {animal.AnimalId.ID}) registered");
#endif
        }
        
        public void Release(IAnimal animal) =>
            ReleaseAnimal(animal);

        public void Release(AnimalType animalType)
        {
            IAnimal unregisterAnimal = GetReleaseReady()
                .OrderByDescending(animal => animal.HappinessFactor.Factor).First();
            
            ReleaseAnimal(unregisterAnimal);
        }

        public IAnimal[] GetAnimals(AnimalType panelAnimalType) =>
            _animals.FindAll(animal => animal.AnimalId.Type == panelAnimalType).ToArray();

        public IEnumerable<AnimalType> GetBreedingReady() =>
            _animals.GroupBy(animal => animal.AnimalId.Type).Where(grouping => grouping.Count() > 1)
                .Select(grouping => grouping.Key);

        public IAnimal GetReleaseReadySingle(AnimalType withType)
        {
            IEnumerable<IAnimal> releaseReady = GetReleaseReady();
            IAnimal releaseReadyAnimal = releaseReady.FirstOrDefault(animal => animal.AnimalId.Type == withType);
            
            if (releaseReadyAnimal is null)
                throw new ArgumentNullException(nameof(releaseReadyAnimal));

            return releaseReadyAnimal;
        }

        public IEnumerable<IAnimal> GetReleaseReady()
        {
            IEnumerable<IAnimal> releaseReady = _animals.Where(IsReleaseReady).ToArray();

            if (releaseReady.Any())
                return releaseReady.Distinct(new AnimalByTypeComparer());

            throw new ArgumentNullException(nameof(releaseReady));
        }

        public BreedingPair SelectPairForBreeding(AnimalType byType)
        {
            IAnimal[] animalsToPair = _animals
                .Where(animal => animal.AnimalId.Type == byType)
                .OrderBy(animal => animal.HappinessFactor.Factor)
                .Take(AnimalsInPair)
                .ToArray();
            return new BreedingPair(animalsToPair[0], animalsToPair[1]);
        }

        private bool IsReleaseReady(IAnimal animal)
        {
            return animal.Stats.Satiety.IsEmpty == false || AllServices.Container.Single<IGlobalSettings>().CanLetHungryAnimalsRelease;
        }

        private void ReleaseAnimal(IAnimal releasedAnimal)
        {
            Unregister(releasedAnimal);
            Released.Invoke(releasedAnimal);

#if DEBUG
            Debug.Log($"Animal {releasedAnimal.AnimalId.Type} (id: {releasedAnimal.AnimalId.ID}) released");
#endif
        }

        private void Unregister(IAnimal animal)
        {
            if (_animals.Contains(animal) == false)
                throw new Exception($"Animal {animal} wasn't registered");

            _animalCounter.Unregister(animal);
            _animals.Remove(animal);
        }
    }
}