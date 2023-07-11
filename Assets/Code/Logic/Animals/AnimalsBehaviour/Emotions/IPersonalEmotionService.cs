namespace Logic.Animals.AnimalsBehaviour.Emotions
{
    public interface IPersonalEmotionService
    {
        void Register(IEmotive emotive);
        void Unregister(IEmotive emotive);
        void Suppress(EmotionId emotion);
        void Show(EmotionId emotion);
    }
}