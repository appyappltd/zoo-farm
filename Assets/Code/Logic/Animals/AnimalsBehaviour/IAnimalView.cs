using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsStateMachine;
using Logic.Movement;

namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimalView
    {
        AnimalId AnimalId { get; }
        IStatsProvider Stats { get; }
        HappinessFactor HappinessFactor { get; }
        AnimalStateMachine StateMachine { get; }
        NavMeshMover Mover { get; }
        AnimalAnimator Animator { get; }
    }
}