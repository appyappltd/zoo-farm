namespace Observables
{
    internal interface IUpdatable<T>
    {
        IObservable<T> Observable { get; }
    }
}