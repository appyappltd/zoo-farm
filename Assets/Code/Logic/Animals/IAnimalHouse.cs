using System;
using Logic.Foods.FoodSettings;
using Logic.Storages;
using UnityEngine;

namespace Logic.Animals
{
    public interface IAnimalHouse
    {
        event Action<IAnimalHouse> BowlEmpty;
        Transform FeedingPlace { get; }
        IInventory Inventory { get; }
        FoodId EdibleFoodType { get; }
    }
}