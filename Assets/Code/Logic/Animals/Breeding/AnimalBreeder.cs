using System;
using System.Collections.Generic;
using System.Linq;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Services.AnimalHouses;
using Services.Effects;
using Tools.Constants;
using UnityEngine;

namespace Logic.Animals.Breeding
{
    public class AnimalBreeder
    {
        private readonly IEffectService _effectService;
        private readonly IGameFactory _gameFactory;
        private readonly IAnimalHouseService _houseService;

        private readonly Dictionary<IAnimal, Action> _disposes = new Dictionary<IAnimal, Action>();
        private readonly List<IAnimal> _breedingReadyAnimals = new List<IAnimal>();
        private readonly List<IAnimal> _remainingBreedingReadyAnimals = new List<IAnimal>();

        private IEnumerable<AnimalType> _typesEnumerator = new List<AnimalType>();

        private Vector2 _pairFoundingDelay = new Vector2(5f, 10f);

        private RoutineSequence _pairFounding;

        public AnimalBreeder(IEffectService effectService, IGameFactory gameFactory, IAnimalHouseService houseService)
        {
            _effectService = effectService;
            _gameFactory = gameFactory;
            _houseService = houseService;

            _pairFounding = new RoutineSequence();
            _pairFounding.WaitForRandomSeconds(_pairFoundingDelay).Then(FindPairs);
        }

        public void Register(IAnimal animal)
        {
            void LocalSatietyOnFull() => OnSatietyFull(animal);
            animal.Stats.Satiety.Full += LocalSatietyOnFull;
            _disposes.Add(animal, () => animal.Stats.Satiety.Full -= LocalSatietyOnFull);
        }

        public void Unregister(IAnimal animal)
        {
            if (_disposes.TryGetValue(animal, out Action dispose))
            {
                dispose.Invoke();

                if (_breedingReadyAnimals.Contains(animal))
                {
                    animal.Emotions.Suppress(EmotionId.Sleeping);
                    _breedingReadyAnimals.Remove(animal);
                }
            }

            throw new ArgumentOutOfRangeException(nameof(animal));
        }

        private void OnSatietyFull(IAnimal animal)
        {
            _breedingReadyAnimals.Add(animal);
        }

        private void FindPairs()
        {
            if (_typesEnumerator.GetEnumerator().MoveNext())
            {
                AnimalType animalType = _typesEnumerator.GetEnumerator().Current;

                if (_houseService.HasEmptyHouse(animalType))
                {
                    _pairFounding.Play();
                    return;
                }
                
                if (TryFindPair(animalType, out AnimalPair animalPair)) 
                    BeginBreeding(animalPair.First, animalPair.Second);
            }
            else
            {
                _typesEnumerator = _breedingReadyAnimals.Select(animal => animal.AnimalId.Type).Distinct();
            }
        }

        private bool TryFindPair(AnimalType breedingAnimalType, out AnimalPair pair)
        {
            IAnimal[] ready = _breedingReadyAnimals.Where(animal => animal.AnimalId.Type == breedingAnimalType && IsBreedingReady(animal)).ToArray();
            pair = new AnimalPair();
            
            if (ready.Count() > AnimalPair.PairCount)
            {
                pair = new AnimalPair(ready[0], ready[1]);
                return true;
            }

            return false;
        }

        private void BeginBreeding(IAnimal first, IAnimal second)
        {
            _breedingReadyAnimals.Remove(first);
            _breedingReadyAnimals.Remove(second);
            
            first.StateMachine.MoveBreeding(second, () => OnBreedingBegin(first, second), () =>  OnBreedingComplete(first, second));
            second.StateMachine.MoveBreeding(first, () => { }, () => { });
        }

        private void OnBreedingComplete(IAnimal first, IAnimal second)
        {
            first.Emotions.Suppress(EmotionId.Sleeping);
            second.Emotions.Suppress(EmotionId.Sleeping);
            
            Vector3 centerAnimalsPosition = GetPositionBetweenAnimals(first.Transform, second.Transform);
            _gameFactory.CreateAnimal(first, centerAnimalsPosition, Quaternion.identity);
        }

        private void OnBreedingBegin(IAnimal first, IAnimal second)
        {
            Vector3 centerAnimalsPosition = GetPositionBetweenAnimals(first.Transform, second.Transform);
            _effectService.SpawnEffect(EffectId.Hearts, centerAnimalsPosition, Quaternion.LookRotation(Vector3.up));
        }

        private bool IsBreedingReady(IAnimal animal) =>
            animal.Stats.Satiety.IsEmpty == false;

        private Vector3 GetPositionBetweenAnimals(Transform first, Transform second) =>
            (first.position - second.position) * Arithmetic.ToHalf + second.position;
    }
}