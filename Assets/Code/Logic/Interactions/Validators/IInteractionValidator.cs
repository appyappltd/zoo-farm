namespace Logic.Interactions.Validators
{
    public interface IInteractionValidator
    {
        public bool IsValid<T>(T target = default);
    }
}