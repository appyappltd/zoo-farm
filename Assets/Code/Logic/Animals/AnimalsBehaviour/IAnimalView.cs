using Logic.Animals.AnimalsBehaviour.AnimalStats;

namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimalView
    {
        AnimalId AnimalId { get; }
        IStatsProvider Stats { get; }
        HappinessFactor HappinessFactor { get; }
    }
}