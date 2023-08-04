using Logic.Payment;
using UnityEngine;

namespace Logic.Player
{
    public class HeroWallet : MonoBehaviour
    {
        private Wallet _wallet;
        public Wallet Wallet => _wallet;

        private void Awake()
        {
            _wallet = new Wallet();
        }
    }
}