using Observables;

namespace Logic.Payment
{
    public interface IWallet
    {
        Observable<int> Account { get; }
        bool TryAdd(int amount);
        bool TrySpend(int amount);
    }
}