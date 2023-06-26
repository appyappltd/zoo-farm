using Progress;

namespace Logic.Animals.AnimalsBehaviour.AnimalStats
{
    public interface IStatsProvider
    {
        IProgressBarView Vitality { get; }
        IProgressBarView Satiety { get; }
        IProgressBarView Peppiness { get; }
    }
}