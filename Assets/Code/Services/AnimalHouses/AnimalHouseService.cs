using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factory;
using JetBrains.Annotations;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;

namespace Services.AnimalHouses
{
    public class AnimalHouseService : IAnimalHouseService
    {
        private const string HouseNotFoundException = "An animal with this Id has not been assigned a home";
        
        private readonly IGameFactory _gameFactory;
        
        private readonly List<AnimalHouse> _animalHouses = new List<AnimalHouse>();
        private readonly Queue<Func<IAnimal>> _queueInHouse = new Queue<Func<IAnimal>>();
        private readonly List<AnimalId> _animalsInQueue = new List<AnimalId>();

        public IReadOnlyList<AnimalId> AnimalsInQueue => _animalsInQueue;

        public void TakeQueueToHouse(AnimalId animalId, Func<IAnimal> callback)
        {
            AnimalHouse freeHouse = GetFreeHouse();

            if (freeHouse is null)
            {
                _queueInHouse.Enqueue(callback);
                _animalsInQueue.Add(animalId);
            }
            else
            {
                IAnimal animal = callback.Invoke();
                TakeHouse(freeHouse, animal);
            }
        }

        public void VacateHouse(AnimalId withAnimalId)
        {
            AnimalHouse attachedHouse =
                _animalHouses.FirstOrDefault(house => house.IsTaken && house.AnimalId.Equals(withAnimalId));

            if (attachedHouse is null)
                throw new NullReferenceException(HouseNotFoundException);

            attachedHouse.DetachAnimal();
        }

        public void RegisterHouse(AnimalHouse house)
        {
            _animalHouses.Add(house);

            if (_queueInHouse.TryDequeue(out Func<IAnimal> callback))
            {
                IAnimal animal = callback.Invoke();
                _animalsInQueue.Remove(animal.AnimalId);
                TakeHouse(house, animal);
            }
        }

        [CanBeNull]
        private AnimalHouse GetFreeHouse() =>
            _animalHouses.FirstOrDefault(house => house.IsTaken == false);

        private void TakeHouse(AnimalHouse builtHouse, IAnimal animal)
        {
            builtHouse.AttachAnimal(animal.AnimalId);
            animal.AttachHouse(builtHouse);
        }
    }
}