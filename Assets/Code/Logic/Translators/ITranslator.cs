namespace Logic.Translators
{
    public interface ITranslator
    {
        bool IsActive { get; } 
        void AddTranslatable(ITranslatable translatable);
        void RemoveTranslatable(ITranslatable translatable);
        void RemoveAllTranslatables();
    }
}