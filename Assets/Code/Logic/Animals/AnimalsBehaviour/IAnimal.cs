using Logic.Animals.AnimalFeeders;
using Logic.Houses;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimal : IAnimalView
    {
        void AttachHouse(AnimalHouse house);
        void Destroy();
        void ForceMove(Transform to);
        void AttachFeeder(AnimalFeeder feeder);
    }
}