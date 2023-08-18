using Logic.Storages;

namespace Logic.Upgrades
{
    public class InventoryImprover : IStatImprover
    {
        private IImprovable _inventory;
        
        public InventoryImprover(IImprovable inventory)
        {
            _inventory = inventory;
        }

        public void Improver(int byAmount)
        {
            
        }
    }
}