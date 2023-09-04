using System;
using System.Collections.Generic;
using System.Linq;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Breeding;
using Services.AnimalHouses;
using Services.Animals;
using Services.Effects;
using Services.Feeders;
using Tools.Constants;
using UnityEngine;
using UnityEngine.Pool;

namespace Services.Breeding
{
    public class AnimalBreedService : IAnimalBreedService
    {
        private readonly IEffectService _effectService;
        private readonly IGameFactory _gameFactory;
        private readonly IAnimalFeederService _feederService;

        private readonly Dictionary<IAnimal, Action> _disposes = new Dictionary<IAnimal, Action>();
        private readonly List<IAnimal> _breedingReadyAnimals = new List<IAnimal>();

        public AnimalBreedService(IEffectService effectService, IGameFactory gameFactory, IAnimalFeederService feederService, IAnimalsService animalsService)
        {
            _effectService = effectService;
            _gameFactory = gameFactory;
            _feederService = feederService;

            animalsService.Registered += Register;
            animalsService.Released += Unregister;
        }

        public void Register(IAnimal animal)
        {
            void OnSatietyFull() => _breedingReadyAnimals.Add(animal);
            void OnSatietyEmpty() => _breedingReadyAnimals.Remove(animal);
            
            animal.Stats.Satiety.Full += OnSatietyFull;
            animal.Stats.Satiety.Empty += OnSatietyEmpty;
            
            _disposes.Add(animal, () =>
            {
                animal.Stats.Satiety.Full -= OnSatietyFull;
                animal.Stats.Satiety.Empty -= OnSatietyEmpty;
            });
        }

        public void Unregister(IAnimal animal)
        {
            if (_disposes.TryGetValue(animal, out Action dispose))
            {
                dispose.Invoke();

                if (_breedingReadyAnimals.Contains(animal))
                    _breedingReadyAnimals.Remove(animal);

                return;
            }

            throw new ArgumentNullException(nameof(dispose));
        }

        public IEnumerable<AnimalType> GetAvailablePairTypes()
        {
            using (ListPool<AnimalType>.Get(out List<AnimalType> result))
            {
                IEnumerable<IGrouping<AnimalType, IAnimal>> groups = _breedingReadyAnimals.GroupBy(animal => animal.AnimalId.Type);
                
                foreach (IGrouping<AnimalType, IAnimal> group in groups)
                    if (group.Count() >= AnimalPair.PairCount)
                        result.Add(group.Key);

                return result;
            }
        }

        public bool TryBreeding(AnimalType breedingAnimalType, out AnimalPair pair)
        {
            IAnimal[] ready = _breedingReadyAnimals.Where(animal => animal.AnimalId.Type == breedingAnimalType).ToArray();

            if (ready.Count() >= AnimalPair.PairCount)
            {
                pair =  new AnimalPair(ready[0], ready[1]);
                return true;
            }

            pair = new AnimalPair();
            return false;
        }

        public void BeginBreeding(AnimalPair pair, Transform at)
        {
            _breedingReadyAnimals.Remove(pair.First);
            _breedingReadyAnimals.Remove(pair.Second);

            BreedingProcess breedingProcess = new BreedingProcess(_effectService, _gameFactory, _feederService, pair, at);
            breedingProcess.Start();
        }
    }
}