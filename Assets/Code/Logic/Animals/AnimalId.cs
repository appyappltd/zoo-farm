using System;
using Logic.Foods.FoodSettings;

namespace Logic.Animals
 {
     public sealed class AnimalId
     {
         public AnimalType Type { get; }
         public int ID { get; }
         public FoodId EdibleFood { get; }

         public AnimalId(AnimalType type, int id, FoodId edibleFood)
         {
             EdibleFood = edibleFood;
             Type = type;
             ID = id;
         }
 
         public override bool Equals(object other) =>
             Equals(other as AnimalId);

         private bool Equals(AnimalId other)
         {
             return other != null &&
                    Type == other.Type &&
                    ID == other.ID &&
                    EdibleFood == other.EdibleFood;
         }
 
         public override int GetHashCode() =>
             HashCode.Combine((int) Type, ID, EdibleFood);
 
         public override string ToString() =>
             $"{Type}, {ID}, {GetHashCode()}, {EdibleFood}";
     }
 }