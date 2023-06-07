namespace Logic.Wallet
{
    public interface IWallet
    {
        Observable.IObservable<int> Account { get; }
        bool TryAdd(int amount);
        bool TrySpend(int amount);
    }
}