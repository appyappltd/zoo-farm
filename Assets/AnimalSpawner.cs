using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AnimalSpawner : MonoBehaviour
{
    [SerializeField] private HandItem _animal;

    public HandItem InstantiateAnimal(Transform parent) => Instantiate(_animal, parent);
}
