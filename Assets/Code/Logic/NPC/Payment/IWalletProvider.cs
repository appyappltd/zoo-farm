namespace Logic.Payment
{
    public interface IWalletProvider
    {
        public IWallet Wallet { get; }
    }
}