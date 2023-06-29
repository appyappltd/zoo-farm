using Logic.Interactions;
using Logic.Inventory;
using Logic.Movement;
using Logic.Translators;
using Player;
using UnityEngine;

namespace Logic.Coins
{
    [RequireComponent(typeof(TriggerObserver))]
    [RequireComponent(typeof(TowardsMover))]
    public class Coin : MonoBehaviour
    {
        [SerializeField, Min(0)] private int _amount = 1;

        private TriggerObserver trigger;
        private TowardsMover mover;

        private void Awake()
        {
            mover = GetComponent<TowardsMover>();
            trigger = GetComponent<TriggerObserver>();

            trigger.Enter += OnEnter;
        }

        private void OnDestroy()
        {
            trigger.Enter -= OnEnter;
        }

        private void OnEnter(GameObject player)
        {
            void OnCollected()
            {
                player.GetComponent<HeroWallet>().Wallet.TryAdd(_amount);
                mover.GotToPlace -= OnCollected;
            }
            mover.GotToPlace += OnCollected;
            mover.Move(player.GetComponent<Storage>().GetItemPlace);
        }
    }
}
