namespace Tutorial
{
    public interface ITutorialModule
    {
        void Play();
        void Complete();
        void AttachNext(ITutorialModule next);
    }
}