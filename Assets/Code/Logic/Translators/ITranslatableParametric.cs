namespace Logic.Translators
{
    public interface ITranslatableParametric<in T> : ITranslatable
    {
        void Play(T from, T to);
    }
}