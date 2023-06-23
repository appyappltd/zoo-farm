using Infrastructure;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.Events;

public class VolunteerSpawner : MonoBehaviour
{
    public event UnityAction<VolunteerMovement> SpawnV;

    [SerializeField] private Vector2 _time = new(5, 40);
    [SerializeField] private Transform _spawnPlace;
    [SerializeField] private GameObject _volunteer;
    [SerializeField] private AnimalSpawner _animalSpawner;

    private void Awake()
    {
        StartCoroutine(CreateVolunteer());
    }

    private IEnumerator CreateVolunteer()
    {
        yield return new WaitForSeconds(5);

        while (true)
        {
            var v = Instantiate(_volunteer,_spawnPlace.localPosition,_spawnPlace.rotation);

            SpawnV?.Invoke(v.GetComponent<VolunteerMovement>());
            var storage = v.GetComponent<Storage>();
            var animal = _animalSpawner.InstantiateAnimal(storage.GetItemPlace);
            v.GetComponent<Inventory>().Add(animal);

            yield return new WaitForSeconds(Random.Range(_time.x, _time.y));
        }
    }
}
