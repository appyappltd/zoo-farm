namespace Logic.AnimalsBehaviour.Emotions
{
    public interface IPersonalEmotionService
    {
        void Register(IEmotive emotive);
        void Unregister(IEmotive emotive);
    }
}