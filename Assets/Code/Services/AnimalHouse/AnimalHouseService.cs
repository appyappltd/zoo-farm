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

        private List<Logic.AnimalHouse> _animalHouses = new List<Logic.AnimalHouse>();
        private Queue<Animal> _queueInHouse = new Queue<Animal>();

        public AnimalHouseService(IGameFactory gameFactory)
        {
            _houseBuilder = new AnimalHouseBuilder(gameFactory);
        }

        public bool TryTakeHouse(Animal withAnimal)
        {
            Logic.AnimalHouse animalHouse = GetFreeHouse();

            if (animalHouse is null)
            {
                _queueInHouse.Enqueue(withAnimal);
            }
            
            return animalHouse is not null;
        }

        private Logic.AnimalHouse GetFreeHouse() =>
            _animalHouses.FirstOrDefault(house => house.IsTaken == false);

        public void BuildHouse()
        {
            Logic.AnimalHouse builtHouse = _houseBuilder.Build(new Vector3(4, 0, 4));
            _animalHouses.Add(builtHouse);

            if (_queueInHouse.TryDequeue(out Animal animal))
            {
                builtHouse.AttachAnimal(animal.AnimalId);
                animal.AttachHouse(builtHouse);
            }
        }
    }
}