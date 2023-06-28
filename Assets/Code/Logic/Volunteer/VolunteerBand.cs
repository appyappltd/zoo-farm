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

        [SerializeField] private List<Transform> _emptyQueue = new();
        [SerializeField] private List<Volunteer> _emptyVolunteers = new();

        public bool CanGiveAnimal() =>
            _volunteers.Count > 0 && _volunteers.First().CanGiveAnimal;

        public HandItem GetAnimal()
        {
            var v = _volunteers.First();
            v.CanGiveAnimal = false;
            _volunteers.Remove(v);
            _emptyVolunteers.Add(v);

            var item = v.GetComponent<Inventory.Inventory>().Remove();
            MoveQueue();
            return item;
        }

        public void AddNewVolunteer(Volunteer volunteer)
        {
            var pos = _transmittingPlace.position + _offset * _queue.Count;
            var t = new GameObject("Queue" + _queue.Count);
            t.transform.position = pos;

            SetVolunteer(volunteer, t.transform);

            var machine = volunteer.GetComponent<VolunteersStateMachine.VolunteerStateMachine>();
            machine.Construct(t.transform, _outPlace);
            machine.Play();
        }

        public void AddVolunteer(Volunteer volunteer)
        {
            var pos = _transmittingPlace.position + _offset * _queue.Count;
            var t = _emptyQueue.First();
            _emptyQueue.Remove(t);
            t.transform.position = pos;

            SetVolunteer(volunteer,t);
        }

        private void SetVolunteer(Volunteer volunteer,Transform t)
        {
            volunteer.GetComponent<Inventory.Inventory>().Add(_animalSpawner.InstantiateAnimal());
            _queue.Add(t.transform);
            _volunteers.Add(volunteer);
            volunteer.CanGiveAnimal = true;
            volunteer.CanTakeAnimal = false;
        }

        [Button("New Volunteer", enabledMode: EButtonEnableMode.Playmode)]
        public void CreateNewVolunteer()
        {
            for (int i = 0; i < _emptyVolunteers.Count; i++)
            {
                var v = _emptyVolunteers[i];
                if (v.CanTakeAnimal)
                {
                    _emptyVolunteers.Remove(v);
                    AddVolunteer(v);
                    return;
                }
            }
            var newV = _spawner.Spawn();
            AddNewVolunteer(newV);
        }

        private void MoveQueue()
        {
            var newLast = _queue.First();
            _queue.Remove(newLast);

            for (int t = 0; t < _queue.Count; t++)
            {
                _queue[t].position = _transmittingPlace.position + _offset * t;
            }
            _emptyQueue.Add(newLast);
        }
    }
}
