using Logic.Animals.AnimalFeeders;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimal : IAnimalView
    {
        void Destroy();
        void ForceMove(Transform to);
        void AttachFeeder(AnimalFeeder feeder);
        bool IsVisible { get; }
    }
}