using Data.ItemsData;
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
using UnityEngine.UIElements;

[RequireComponent(typeof(RunTranslator))]
[RequireComponent(typeof(Delay))]
public class AnimalLiberator : MonoBehaviour
{
    public event Action TakeAnimal = () => { };

    private RunTranslator translator;
    private TriggerObserver trigger;
    private VisualTranslatorsSpawner spawner;
    private int left;

    private void Awake()
    {
        translator = GetComponent<RunTranslator>();
        trigger = GetComponent<TriggerObserver>();
        trigger.Enter += Init;

        GetComponent<Delay>().Complete += OnComplete;
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

    private void OnComplete(GameObject player)
    {
        var inventory = player.GetComponent<Inventory>();

        if (inventory.CanGiveItem(CreatureType.Animal))
        {
            var item = inventory.Remove();
            var mover = item.GetComponent<IMover>();

            mover.Move(transform);
            mover.GotToPlace += () =>
            {
                TakeAnimal.Invoke();
                Destroy(item.gameObject);
            };
        }
    }

    private IEnumerator GetCoins(GameObject player)
    {
        var wallet = player.GetComponent<HeroWallet>().Wallet;
        while (left > 0)
        {
            if (!wallet.TrySpend(1))
                break;
            left--;

            if (left == 0)

            yield return new WaitForSeconds(.1f);
        }
    }
}
