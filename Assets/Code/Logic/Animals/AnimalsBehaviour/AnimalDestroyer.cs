using Observer;

namespace Logic.Animals.AnimalsBehaviour
{
    public class AnimalDestroyer : ObserverTarget<IAnimal, ITriggerObserver>
    {
        protected override void OnTargetEntered(IAnimal animal)
        {
            animal.Destroy();
        }
    }
}