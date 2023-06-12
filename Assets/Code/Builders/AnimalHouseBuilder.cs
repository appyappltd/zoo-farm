using Infrastructure.Factory;
using Logic;
using UnityEngine;

namespace Builders
{
    public class AnimalHouseBuilder
    {
        private readonly IGameFactory _gameFactory;

        public AnimalHouseBuilder(IGameFactory gameFactory)
        {
            _gameFactory = gameFactory;
        }

        public AnimalHouse Build(Vector3 at)
        {
            return _gameFactory.CreateAnimalHouse(at).GetComponent<AnimalHouse>();
        }
    }
}