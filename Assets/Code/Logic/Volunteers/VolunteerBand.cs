using System;
using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.VolunteersStateMachine;
using NaughtyAttributes;
using Services;
using UnityEngine;

namespace Logic.Volunteers
{
    public class VolunteerBand : MonoBehaviour, IGetItem
    {
        [SerializeField] private Vector3 _offset = new(1f, .0f, .0f);
        [SerializeField] private AnimalSpawner _animalSpawner;

        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private Transform _transmittingPlace;
        [SerializeField] private Transform _outPlace;
        [SerializeField] private Transform _lookAtPlace;

        [SerializeField] private List<Transform> _queue = new();
        [SerializeField] private List<Volunteer> _volunteers = new();

        [SerializeField] private List<Transform> _emptyQueue = new();
        [SerializeField] private List<Volunteer> _emptyVolunteers = new();

        private IGameFactory _gameFactory;

        public event Action<IItem> Removed;
        
        public int VolunteersCount => _volunteers.Count;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
        }

        public IItem Get()
        {
            Volunteer volunteer = _volunteers.First();
            _volunteers.Remove(volunteer);
            _emptyVolunteers.Add(volunteer);

            IItem item = volunteer.Inventory.Get();
            MoveQueue();
            return item;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;

            if (_volunteers.Count <= 0) 
                return false;

            Volunteer volunteer = _volunteers[0];
            return volunteer.IsReadyTransmitting && volunteer.Inventory.TryPeek(ItemId.Animal, out item);
        }

        public bool TryGet(ItemId byId, out IItem result)
        {
            if (TryPeek(byId, out result))
            {
                result = Get();
                return true;
            }

            return false;
        }

        public void AddNewVolunteer(Volunteer volunteer)
        {
            var pos = _transmittingPlace.position + _offset * _queue.Count;
            var t = new GameObject("Queue" + _queue.Count);
            t.transform.position = pos;

            SetVolunteer(volunteer, t.transform);

            VolunteerStateMachine machine = volunteer.StateMachine;
            machine.Construct(t.transform, _outPlace, _transmittingPlace, volunteer);
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

        [Button("New Volunteer", enabledMode: EButtonEnableMode.Playmode)]
        public void CreateNewVolunteer()
        {
            for (int i = 0; i < _emptyVolunteers.Count; i++)
            {
                Volunteer volunteer = _emptyVolunteers[i];
                
                if (volunteer.IsFree)
                {
                    _emptyVolunteers.Remove(volunteer);
                    AddVolunteer(volunteer);
                    return;
                }
            }
            
            GameObject newVolunteer = _gameFactory.CreateVolunteer(_spawnPlace.position, _container);
            AddNewVolunteer(newVolunteer.GetComponent<Volunteer>());
        }

        private void SetVolunteer(Volunteer volunteer, Transform t)
        {
            HandItem animal = _animalSpawner.InstantiateAnimal(volunteer.transform.position);
            volunteer.Inventory.Add(animal);
            _queue.Add(t.transform);
            _volunteers.Add(volunteer);
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
