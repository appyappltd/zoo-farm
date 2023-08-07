using Logic.Interactions;
using Logic.Movement;
using Logic.Payment;
using Logic.Player;
using Logic.Translators;
using Services.Pools;
using UnityEngine;

namespace Logic.Coins
{
    [RequireComponent(typeof(HeroInteraction))]
    [RequireComponent(typeof(ItemMover))]
    public class CollectibleCoin : MonoBehaviour, IPoolable
    {
        [SerializeField] private HeroInteraction _playerInteraction;
        [SerializeField] private TranslatableAgent _translatableAgent;
        [SerializeField, Min(0)] private int _amount = 1;

        private IItemMover _itemMover;
        private IWallet _wallet;

        public TranslatableAgent TranslatableAgent => _translatableAgent;
        public GameObject GameObject => gameObject;
        public IItemMover ItemMover => _itemMover;

        private void Awake()
        {
            _itemMover ??= GetComponent<IItemMover>();
            _playerInteraction.Interacted += OnEnter;
        }

        private void OnDestroy() =>
            _playerInteraction.Interacted -= OnEnter;

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
