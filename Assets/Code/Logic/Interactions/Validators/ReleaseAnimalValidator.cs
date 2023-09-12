using Data.AnimalCounter;
using Logic.Animals;
using Services.Animals;
 using UnityEngine;
 
 namespace Logic.Interactions.Validators
 {
     public class ReleaseAnimalValidator : MonoBehaviour, IInteractionValidator
     {
         private AnimalType _validateType;
         private IAnimalCounter _counter;
 
         public void Construct(IAnimalsService animalsService, AnimalType validateType)
         {
             _validateType = validateType;
             _counter = animalsService.AnimalCounter;
         }
 
         public bool IsValid<T>(T target = default) =>
             _counter.GetAnimalCountData(_validateType).ReleaseReady > 0;
     }
 }