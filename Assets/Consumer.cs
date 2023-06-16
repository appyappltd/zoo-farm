using Infrastructure.Factory;
using Logic.Interactions;
using Logic.Spawners;
using Logic.Translators;
using Player;
using Pool;
using Services;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Delay))]
public class Consumer : MonoBehaviour
{
    public event Action Bought = () => { };

    [SerializeField, Min(1)] private int _cost = 10;
    [SerializeField, Min(.0f)] private float _time = .1f;

    private VisualTranslatorsSpawner spawner;
    private RunTranslator translator;
    private TriggerObserver trigger;
    private ITranslatable translatable;

    private int left;

    private void Awake()
    {
        left = _cost;

        translator = GetComponent<RunTranslator>();
        trigger = GetComponent<TriggerObserver>();

        trigger.Enter += Init;
        trigger.Exit += _ => StopAllCoroutines();

        GetComponent<Delay>().Complete += player => StartCoroutine(TakeCoins(player));
    }

    private void Init(GameObject player)
    {
        spawner = new VisualTranslatorsSpawner(() => AllServices.Container.Single<IGameFactory>().CreateVisual(VisualType.Money,
                                                                                                       Quaternion.identity,
                                                                                                       new GameObject("Coins").transform),
                                                                                                       10, translator, player.transform,
                                                                                                       transform);
        trigger.Enter -= Init;
    }

    private IEnumerator TakeCoins(GameObject player)
    {
        var wallet = player.GetComponent<HeroWallet>().Wallet;
        while (left > 0)
        {
            if (!wallet.TrySpend(1))
                break;
            left--;

            translatable = spawner.Spawn().MainTranslatable;
            if (left == 0)
                translatable.End += Buy;

            yield return new WaitForSeconds(_time);
        }
    }

    private void Buy(ITranslatable _)
    {
        Bought.Invoke();
        translatable.End -= Buy;
    }
}
