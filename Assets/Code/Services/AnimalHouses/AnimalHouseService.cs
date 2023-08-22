using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Infrastructure.Factory;
using JetBrains.Annotations;
using Logic.Breeding;
using UnityEngine;

namespace Services.AnimalHouses
{
    public class AnimalHouseService : IAnimalHouseService
    {
        private const string HouseNotFoundException = "An animal with this Id has not been assigned a home";

        private readonly IGameFactory _gameFactory;

        private readonly List<AnimalHouse> _animalHouses = new List<AnimalHouse>();
        private readonly List<BreedingHouse> _breedingHouses = new List<BreedingHouse>();
        private readonly List<QueueToHouse> _animalsInQueue = new List<QueueToHouse>();
        private readonly List<QueueToHouse> _animalsInPriorityQueue = new List<QueueToHouse>();
        private readonly Queue<IAnimalHouse> _feedQueue = new Queue<IAnimalHouse>();

        public IReadOnlyList<QueueToHouse> AnimalsInQueue => _animalsInQueue;
        public int AnimalsInHouseQueueCount => _animalsInQueue.Count;

        public void RegisterHouse(IAnimalHouse house)
        {
            switch (house)
            {
                case AnimalHouse animalHouse:
                    RegisterAnimalHouse(animalHouse);
                    break;
                case BreedingHouse breedingHouse:
                    RegisterBreedingHouse(breedingHouse);
                    break;
            }
        }

        private void RegisterBreedingHouse(BreedingHouse breedingHouse)
        {
            if (breedingHouse.IsServedByKeeper)
            {
                breedingHouse.BowlEmpty += AddToFeedQueue;
                AddToFeedQueue(breedingHouse);
            }

            _breedingHouses.Add(breedingHouse);
        }

        private void RegisterAnimalHouse(AnimalHouse animalHouse)
        {
            if (_animalHouses.Contains(animalHouse))
                throw new Exception($"House {animalHouse} already registered");

            animalHouse.BowlEmpty += AddToFeedQueue;
            _animalHouses.Add(animalHouse);
            AddToFeedQueue(animalHouse);
            TryTakeHouse(animalHouse);
        }

        public void TakeQueueToHouse(QueueToHouse queueToHouse, bool isHighPriority = false)
        {
            Debug.Log("TakeQueueToHouse");
            
            AnimalHouse freeHouse = GetFreeHouseFor(queueToHouse.Animal.AnimalId.Type);

            List<QueueToHouse> targetQueue = GetQueueByPriority(isHighPriority);
            
            if (freeHouse is null)
            {
                Debug.Log("Add to queue");
                targetQueue.Add(queueToHouse);
            }
            else
            {
                IAnimal animal = queueToHouse.OnTakeHouse.Invoke();
                TakeHouse(freeHouse, animal);
            }
        }

        public void VacateHouse(AnimalId withAnimalId)
        {
            AnimalHouse attachedHouse =
                _animalHouses.FirstOrDefault(house => house.IsTaken && house.AnimalId.Type.Equals(withAnimalId.Type));

            if (attachedHouse is null)
            {
#if DEBUG
                return;
#endif
                throw new NullReferenceException(HouseNotFoundException);
            }

            attachedHouse.DetachAnimal();
            attachedHouse.Clear();
            TryTakeHouse(attachedHouse);
        }

        public bool TryGetNextFeedHouse(out IAnimalHouse feedHouse)
        {
            bool tryGetNextFeedHouse = _feedQueue.TryDequeue(out feedHouse);

            if (tryGetNextFeedHouse)
            {
                Debug.Log($"FeedQueue dequeued, remaining {_feedQueue.Count}");
            }
            
            return tryGetNextFeedHouse;
        }

        public bool TryGetNextAnimalIdInQueue(out AnimalId animalId)
        {
            if (_animalsInQueue.Count > 0)
            {
                animalId = _animalsInQueue[0].Animal.AnimalId;
                return true;
            }

            animalId = null;
            return false;
        }

        public IEnumerable<AnimalType> GetAnimalTypesInHouseQueue(bool isHighPriority = false) =>
            GetQueueByPriority(isHighPriority).Select(queue => queue.Animal.AnimalId.Type).Distinct();

        private List<QueueToHouse> GetQueueByPriority(bool isHighPriority) =>
            isHighPriority ? _animalsInPriorityQueue : _animalsInQueue;

        private void TryTakeHouse(AnimalHouse house)
        {
            if (TryGetAnimalFromQueue(_animalsInPriorityQueue, house.ForAnimal, out QueueToHouse animalQueue))
            {
                SendAnimalHome(house, _animalsInPriorityQueue, animalQueue);
            }
            else if (TryGetAnimalFromQueue(_animalsInQueue, house.ForAnimal, out animalQueue))
            {
                SendAnimalHome(house, _animalsInQueue, animalQueue);
            }
        }

        private void SendAnimalHome(AnimalHouse house, List<QueueToHouse> queue, QueueToHouse animalQueue)
        {
            IAnimal animal = animalQueue.OnTakeHouse.Invoke();
            queue.Remove(animalQueue);
            TakeHouse(house, animal);
        }

        private bool TryGetAnimalFromQueue(List<QueueToHouse> queue, AnimalType forHouseType, out QueueToHouse queueToHouse)
        {
            queueToHouse = new QueueToHouse();
            
            for (var index = 0; index < queue.Count; index++)
            {
                QueueToHouse queueAnimal = queue[index];

                if (queueAnimal.Animal.AnimalId.Type != forHouseType)
                    continue;

                queueToHouse = queueAnimal;
                return true;
            }

            return false;
        }

        private void AddToFeedQueue(IAnimalHouse house) =>
            _feedQueue.Enqueue(house);

        [CanBeNull]
        private AnimalHouse GetFreeHouseFor(AnimalType animalIdType) =>
            _animalHouses.FirstOrDefault(house => house.IsTaken == false && house.ForAnimal == animalIdType);

        private void TakeHouse(AnimalHouse builtHouse, IAnimal animal)
        {
            builtHouse.AttachAnimal(animal.AnimalId);
            animal.AttachHouse(builtHouse);
        }
    }
}