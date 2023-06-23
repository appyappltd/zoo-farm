using System.Collections;
using Logic.Animals;
using Logic.Inventory;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Volunteer
{
    public class VolunteerSpawner : MonoBehaviour
    {
        public event UnityAction<VolunteerMovement> SpawnV;

        [SerializeField] private Vector2 _time = new(5, 40);
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private GameObject _volunteer;
        [SerializeField] private AnimalSpawner _animalSpawner;

        public bool isReady = true;
        private bool canSpawn = true;

        public void Awake()
        {
            TrySpawn();
        }

        public void TrySpawn()
        {
            if (!canSpawn || !isReady)
                return;
            Spawn();
        }

        private void Spawn()
        {
            var volunteer = Instantiate(_volunteer, _spawnPlace.position, _spawnPlace.rotation);
            volunteer.transform.SetParent(transform);
            SpawnV?.Invoke(volunteer.GetComponent<VolunteerMovement>());
            var storage = volunteer.GetComponent<Storage>();
            var animal = _animalSpawner.InstantiateAnimal(storage.GetItemPlace);
            volunteer.GetComponent<Inventory.Inventory>().Add(animal);

            isReady = false;
            canSpawn = false;

            StartCoroutine(ReloadSpawn());
        }

        private IEnumerator ReloadSpawn()
        {
            yield return new WaitForSeconds(Random.Range(_time.x, _time.y));
            canSpawn = true;
            TrySpawn();
        }
    }
}
