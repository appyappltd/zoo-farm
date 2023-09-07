using Data;
using Data.AnimalCounter;
using Logic.Animals;
 using Logic.Storages;
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
 
         public bool IsValid(IInventory inventory = default) =>
             _counter.GetAnimalCountData(_validateType).ReleaseReady > 0;
     }
 }