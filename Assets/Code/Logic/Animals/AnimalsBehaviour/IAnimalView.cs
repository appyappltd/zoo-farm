using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Animals.AnimalsStateMachine;
using Logic.Movement;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public interface IAnimalView
    {
        Transform Transform { get; }
        AnimalId AnimalId { get; }
        IStatsProvider Stats { get; }
        HappinessFactor HappinessFactor { get; }
        AnimalStateMachine StateMachine { get; }
        NavMeshMover Mover { get; }
        AnimalAnimator Animator { get; }
        PersonalEmotionService Emotions { get; }
    }
}