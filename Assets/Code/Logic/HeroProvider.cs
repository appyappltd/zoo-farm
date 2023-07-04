using Logic.Payment;
using Logic.Storages;
using Player;
using UnityEngine;

namespace Logic
{
    public class HeroProvider : MonoBehaviour
    {
        [SerializeField] private HeroWallet _heroWallet;
        [SerializeField] private Inventory _heroInventory;

        public IWallet Wallet => _heroWallet.Wallet;
        public Inventory Inventory => _heroInventory;
    }
}