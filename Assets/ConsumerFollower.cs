using Infrastructure.Factory;
using Services;
using UnityEngine;

public class ConsumerFollower : MonoBehaviour
{
    [SerializeField] private Consumer _consumer;

    private void Awake() => _consumer.Bought += OnCreate;

    private void OnCreate()
    {
        AllServices.Container.Single<IGameFactory>().CreateAnimalHouse(transform.position);
        _consumer.Bought -= OnCreate;
    }
}
