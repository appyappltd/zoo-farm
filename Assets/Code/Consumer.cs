using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Spawners;
using Logic.Translators;
using Player;
using Services;
using System;
using System.Collections;
using Logic.Wallet;
using Observables;
using UnityEngine;

[RequireComponent(typeof(RunTranslator))]
[RequireComponent(typeof(Delay))]
public class Consumer : MonoBehaviour
{
    public event Action Bought = () => { };

    [SerializeField, Min(0f)] private float _time = .1f;

    private int _defaultCost;
    private VisualTranslatorsSpawner spawner;
    private RunTranslator translator;
    private TriggerObserver trigger;
    private Wallet wallet;
    private Observable<int> leftCoinsToPay;

    public Observables.IObservable<int> LeftCoinsToPay => leftCoinsToPay;

    private void Awake()
    {
        leftCoinsToPay = new Observable<int>();

        translator = GetComponent<RunTranslator>();
        trigger = GetComponent<TriggerObserver>();

        trigger.Enter += Init;
        trigger.Exit += _ => StopAllCoroutines();

        GetComponent<Delay>().Complete += _ => StartCoroutine(TakeCoins());
    }

    public void SetCost(int buildCost)
    {
        _defaultCost = buildCost;
        leftCoinsToPay.Value = buildCost;
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
        while (leftCoinsToPay.Value > 0)
        {
            if (!wallet.TrySpend(1))
                break;

            leftCoinsToPay.Value--;

            var translatable = spawner.Spawn().MainTranslatable;

            if (leftCoinsToPay.Value == 0)
                translatable.End += Buy;
            
            yield return new WaitForSeconds(_time);
        }
    }

    private void Buy(ITranslatable translatable)
    {
        Bought.Invoke();
        translatable.End -= Buy;
        leftCoinsToPay.Value = _defaultCost;
    }
}
