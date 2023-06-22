using System;
using System.Collections.Generic;
using System.Linq;
using Builders;
using Infrastructure.Factory;
using Logic.AnimalsBehaviour;
using UnityEngine;

namespace Services.AnimalHouse
{
    public class AnimalHouseService : IAnimalHouseService
    {
        private readonly IGameFactory _gameFactory;
        private readonly AnimalHouseBuilder _houseBuilder;

        private readonly List<Logic.AnimalHouse> _animalHouses = new List<Logic.AnimalHouse>();
        private readonly Queue<Func<Animal>> _queueInHouse = new Queue<Func<Animal>>();

        public AnimalHouseService(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
            _houseBuilder = new AnimalHouseBuilder();
        }

        public void TakeQueueToHouse(Func<Animal> callback)
        {
            Logic.AnimalHouse animalHouse = GetFreeHouse();

            if (animalHouse is null)
            {
                _queueInHouse.Enqueue(callback);
            }
            else
            {
                Animal animal = callback.Invoke();
                TakeHouse(animalHouse, animal);
            }
        }

        private Logic.AnimalHouse GetFreeHouse() =>
            _animalHouses.FirstOrDefault(house => house.IsTaken == false);

        public void BuildHouse(Vector3 at)
        {
            GameObject houseObject = _gameFactory.CreateAnimalHouse(at);
            Logic.AnimalHouse builtHouse = houseObject.GetComponent<Logic.AnimalHouse>();
            _animalHouses.Add(builtHouse);

            if (_queueInHouse.TryDequeue(out Func<Animal> callback))
            {
                Animal animal = callback.Invoke();
                TakeHouse(builtHouse, animal);
            }
        }

        private void TakeHouse(Logic.AnimalHouse builtHouse, Animal animal)
        {
            builtHouse.AttachAnimal(animal.AnimalId);
            animal.AttachHouse(builtHouse);
        }
    }
}