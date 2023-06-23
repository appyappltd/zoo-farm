using Logic.Interactions;
using Logic.Translators;
using Player;
using UnityEngine;

[RequireComponent(typeof(TriggerObserver))]
[RequireComponent(typeof(TowardsMover))]
[RequireComponent(typeof(Storage))]
public class Coin : MonoBehaviour
{
    [SerializeField, Min(0)] private int _amount = 1;
    [SerializeField] private TranslatableAgent _translatableAgent;
    [SerializeField] private Collider _collider;
    
    private TriggerObserver trigger;
    private TowardsMover mover;

    private void Awake()
    {
        mover = GetComponent<TowardsMover>();
        trigger = GetComponent<TriggerObserver>();

        trigger.Enter += OnEnter;
        
        _translatableAgent.MainTranslatable.End += OnEndTranslate;
        _translatableAgent.MainTranslatable.Begin += OnBeginTranslate;
    }

    private void OnDestroy()
    {
        trigger.Enter -= OnEnter;
    }

    private void OnEndTranslate(ITranslatable _)
    {
        _collider.enabled = true;
    }
    
    private void OnBeginTranslate(ITranslatable _)
    {
        _collider.enabled = false;
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
