using Logic.AnimalsBehaviour.AnimalStats;

namespace Logic.AnimalsBehaviour
{
    public interface IAnimalView
    {
        AnimalId AnimalId { get; }
        StatsProvider Stats { get; }
    }
}