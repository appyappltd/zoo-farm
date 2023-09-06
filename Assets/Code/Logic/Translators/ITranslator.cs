namespace Logic.Translators
{
    public interface ITranslator
    {
        bool IsActive { get; } 
        void Add(ITranslatable translatable);
        void Remove(ITranslatable translatable);
        void RemoveAll();
    }
}