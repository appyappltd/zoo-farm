using Logic;
using Ui;

namespace Builders
{
    public class AnimalHouseBuilder
    {
        public void Build(AnimalHouse house)
        {
            NeedIconView needIcon = house.GetComponentInChildren<NeedIconView>();
            Bowl bowl = house.GetComponentInChildren<Bowl>();
            needIcon.Construct(bowl.FoodBar.Current, bowl.FoodBar.Max);
        }
    }
}