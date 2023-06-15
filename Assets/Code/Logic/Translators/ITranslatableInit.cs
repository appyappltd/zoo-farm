namespace Logic.Translators
{
    public interface ITranslatableInit<in T> : ITranslatable
    {
        void Init(T from, T to);
    }
}