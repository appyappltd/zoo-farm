namespace Logic.Translators
{
    public interface ITranslator
    {
        bool IsActive { get; } 
        void AddTranslatable(ITranslatable translatable);
    }
}