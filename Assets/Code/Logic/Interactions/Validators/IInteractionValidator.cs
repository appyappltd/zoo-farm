using Logic.Storages;

namespace Logic.Interactions.Validators
{
    public interface IInteractionValidator
    {
        public bool IsValid(IInventory inventory = default);
    }
}