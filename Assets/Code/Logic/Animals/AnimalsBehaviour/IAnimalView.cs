using Logic.Animals.AnimalsBehaviour.AnimalStats;
using StateMachineBase;

namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimalView
    {
        AnimalId AnimalId { get; }
        IStatsProvider Stats { get; }
        HappinessFactor HappinessFactor { get; }
        StateMachine StateMachine { get; }
    }
}