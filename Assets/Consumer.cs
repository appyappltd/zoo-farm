using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Spawners;
using Logic.Translators;
using Player;
using Services;
using System;
using System.Collections;
using Logic.Wallet;
using UnityEngine;

[RequireComponent(typeof(RunTranslator))]
[RequireComponent(typeof(Delay))]
public class Consumer : MonoBehaviour
{
    public event Action Bought = () => { };

    [SerializeField, Min(1)] private int _cost = 10;
    [SerializeField, Min(0f)] private float _time = .1f;

    private VisualTranslatorsSpawner spawner;
    private RunTranslator translator;
    private TriggerObserver trigger;
    private int leftCoinsToPay;
    private Wallet wallet;

    private void Awake()
    {
        leftCoinsToPay = _cost;

        translator = GetComponent<RunTranslator>();
        trigger = GetComponent<TriggerObserver>();

        trigger.Enter += Init;
        trigger.Exit += _ => StopAllCoroutines();

        GetComponent<Delay>().Complete += _ => StartCoroutine(TakeCoins());
    }

    private void Init(GameObject player)
    {
        spawner = new VisualTranslatorsSpawner(() =>
                AllServices.Container.Single<IGameFactory>().CreateVisual(VisualType.Money,
                    Quaternion.identity,
                    new GameObject("Coins").transform),
            10, translator, player.transform,
            transform);
        
        wallet = player.GetComponent<HeroWallet>().Wallet;
        
        trigger.Enter -= Init;
    }

    private IEnumerator TakeCoins()
    {
        leftCoinsToPay = _cost;

        while (leftCoinsToPay > 0)
        {
            if (!wallet.TrySpend(1))
                break;
            leftCoinsToPay--;

            var translatable = spawner.Spawn().MainTranslatable;
            
            if (leftCoinsToPay == 0)
                translatable.End += Buy;

            yield return new WaitForSeconds(_time);
        }
    }

    private void Buy(ITranslatable translatable)
    {
        Bought.Invoke();
        translatable.End -= Buy;
    }
}
