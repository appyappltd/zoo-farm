using Logic.Interactions;
using Player;
using UnityEngine;

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

    private void OnEnter(GameObject player)
    {
        trigger.Enter -= OnEnter;

        mover.GotToPlace += () =>
        {
            player.GetComponent<HeroWallet>().Wallet.TryAdd(_amount);
            Destroy(gameObject);
        };
        mover.Move(player.GetComponent<Inventory>().DefItemPlace);
    }
}
