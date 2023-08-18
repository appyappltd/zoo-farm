using System;
using Logic.Player;
using Logic.Storages;

namespace Logic.Upgrades
{
    public class MaxInventoryWeightImprover : HeroUpgrade
    {
        private const string InterfaceImprovableCastException = "Hero inventory do not implement an interface IImprovable";

        protected override void OnAwake()
        {
            
        }

        protected override IImprovable GetImprovableFrom(Hero hero) =>
            hero.Inventory as IImprovable ?? throw new InvalidCastException(InterfaceImprovableCastException);

        protected override void OnImproved()
        {
            
        }
    }
}