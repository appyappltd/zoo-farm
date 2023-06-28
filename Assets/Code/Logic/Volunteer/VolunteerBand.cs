using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using Logic.Animals;
using Logic.Inventory;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Volunteer
{
    public class VolunteerBand : MonoBehaviour
    {
        [SerializeField, Min(.0f)] private Vector3 _offset = new(1f, .0f, .0f);
        [SerializeField] private VolunteerSpawner _spawner;
        [SerializeField] private AnimalSpawner _animalSpawner;

        [SerializeField] private TransmittingAnimals _transmitting;

        [SerializeField] private Transform _transmittingPlace;
        [SerializeField] private Transform _outPlace;

        [SerializeField] private List<Transform> _queue = new();
        [SerializeField] private List<Volunteer> _volunteers = new();


        public bool CanGiveAnimal() =>
            _volunteers.Count > 0 && _volunteers.First().CanGiveAnimal;

        public HandItem GetAnimal()
        {
            //var v = _volunteers.First();
            //var item = v.GetComponent<Inventory.Inventory>().Remove();
            MoveQueue();
            return _volunteers.First().GetComponent<Inventory.Inventory>().Remove();
        }

        public void AddVolunteer(Volunteer volunteer)
        {
            var t = new GameObject();
            volunteer.GetComponent<Inventory.Inventory>().Add(_animalSpawner.InstantiateAnimal());
            _queue.Add(t.transform);
            _volunteers.Add(volunteer);

            var machine = volunteer.GetComponent<VolunteersStateMachine.VolunteerStateMachine>();
            machine.Construct(t.transform, _outPlace);
            machine.Play();
        }

        [Button("New Volunteer", enabledMode: EButtonEnableMode.Playmode)]
        public void CreateNewVolunteer()
        {
            var v = _spawner.Spawn();
            AddVolunteer(v);
        }

        [Button("Move Queue", enabledMode: EButtonEnableMode.Playmode)]
        private void MoveQueue()
        {
            var newLast = _queue.First();
            _queue.Remove(newLast);

            for (int t = 0; t < _queue.Count; t++)
            {
                _queue[t].position = _transmittingPlace.position + _offset * t;
            }
            
            newLast.position = _transmittingPlace.position + _offset * (_queue.Count + 1);
            _queue.Add(newLast);
        }
    }
}
