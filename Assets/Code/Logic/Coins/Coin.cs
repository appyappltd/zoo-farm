using Logic.Interactions;
using Logic.Movement;
using Logic.Payment;
using Logic.Player;
using UnityEngine;

namespace Logic.Coins
{
    [RequireComponent(typeof(PlayerInteraction))]
    [RequireComponent(typeof(ItemMover))]
    public class Coin : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _amount = 1;
        [SerializeField] private PlayerInteraction _playerInteraction;
        
        private IItemMover _itemMover;
        private IWallet _wallet;

        private void Awake()
        {
            _itemMover = GetComponent<IItemMover>();
            _playerInteraction ??= GetComponent<PlayerInteraction>();

            _playerInteraction.Interacted += OnEnter;
        }

        private void OnDestroy()
        {
            _playerInteraction.Interacted -= OnEnter;
        }

        private void OnEnter(Hero hero)
        {
            _wallet = hero.Wallet;
            _itemMover.Ended += OnCollected;
            _itemMover.Move(hero.transform);
        }
        
        private void OnCollected()
        {
            _wallet.TryAdd(_amount);
            _itemMover.Ended -= OnCollected;
        }
    }
}
