using UnityEngine;

namespace Logic.Payment
{
    public class WalletHolder : MonoBehaviour, IWalletProvider
    {
        private IWallet _wallet;
        public IWallet Wallet => _wallet;

        private void Awake()
        {
            Construct(new Wallet());
        }

        public void Construct(IWallet wallet)
        {
            _wallet = wallet;
        }
    }
}