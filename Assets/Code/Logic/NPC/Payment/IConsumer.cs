using System;

namespace Logic.Payment
{
    public interface IConsumer
    {
        event Action Bought;
        Observables.IObservable<int> LeftCoinsToPay { get; }
        void SetCost(int buildCost);
    }
}