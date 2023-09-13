using System;
using Logic.Animals.AnimalsBehaviour;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class AnimalSatietyValidator : MonoBehaviour, IInteractionValidator
    {
        public bool IsValid<T>(T target = default)
        {
            if (target is IAnimal animal)
                return animal.Stats.Satiety.IsEmpty;

            throw new ArgumentOutOfRangeException(nameof(target));
        }
    }
}