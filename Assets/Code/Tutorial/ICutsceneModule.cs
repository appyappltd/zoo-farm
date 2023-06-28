namespace Tutorial
{
    public interface ICutsceneModule
    {
        void Play();
        void Complete();
        void AttachNext(ICutsceneModule next);
    }
}