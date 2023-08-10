using System;

namespace Logic.Animals
 {
     public sealed class AnimalId
     {
         private const string ComparedValueIsNull = "Compared value is null";
         
         public AnimalType Type { get; }
         public int ID { get; }
 
         public AnimalId(AnimalType type, int id)
         {
             Type = type;
             ID = id;
         }
 
         public override bool Equals(object other)
         {
             if (other is null)
                 throw new NullReferenceException(ComparedValueIsNull);
 
             return GetHashCode() == other.GetHashCode();
         }
 
         public override int GetHashCode() =>
             HashCode.Combine((int) Type, ID);
 
         public override string ToString()
         {
             return $"{Type} {ID}, {GetHashCode()}";
         }
     }
 }