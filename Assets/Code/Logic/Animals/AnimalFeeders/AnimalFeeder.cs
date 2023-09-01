using System;
using System.Collections.Generic;
using Logic.Foods.FoodSettings;

namespace Logic.Animals.AnimalFeeders
{
    public class AnimalFeeder
    {
        private readonly Queue<Bowl> _bowls;
        private readonly FoodId _foodId;
        
        public FoodId FoodId => _foodId;
        
        public AnimalFeeder(IEnumerable<Bowl> bowls, FoodId foodId)
        {
            _bowls = new Queue<Bowl>(bowls);
            _foodId = foodId;
        }

        public bool TryGetFreeBowl(out Bowl bowl) =>
            _bowls.TryDequeue(out bowl);

        public void VacateBowl(Bowl bowl)
        {
            if (_bowls.Contains(bowl))
                throw new ArgumentOutOfRangeException(nameof(bowl));
            
            _bowls.Enqueue(bowl);
        }
    }
}