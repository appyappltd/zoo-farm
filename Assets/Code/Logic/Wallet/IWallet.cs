using Observables;

namespace Logic.Wallet
{
    public interface IWallet
    {
        Observable<int> Account { get; }
        bool TryAdd(int amount);
        bool TrySpend(int amount);
    }
}