using System;
using System.Collections.Generic;
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
        [SerializeField] private AnimalSpawner _animalSpawner;

        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private Transform _outPlace;

        [SerializeField] private Transform[] _queueTransforms;
        [SerializeField] private List<Volunteer> _volunteers = new();

        private IGameFactory _gameFactory;
        private Queue _queue;
        private Volunteer _volunteerCashed;
        private int _volunteersInQueue;
        
        public event Action<IItem> Removed;
        
        public int VolunteersCount => _volunteersInQueue;
        public int MaxVolunteers => _queueTransforms.Length - 2;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _queue = new Queue(_queueTransforms);
        }

        public IItem Get()
        {
            IItem item = _volunteerCashed.Inventory.Get();
            MoveQueue();
            _volunteersInQueue--;
            return item;
        }

        public bool TryPeek(ItemId byId, out IItem item)
        {
            item = null;

            if (_volunteers.Count <= 0) 
                return false;

            _volunteerCashed = _volunteers.Find(volunteer => volunteer.IsReadyTransmitting);
            return _volunteerCashed is not null && _volunteerCashed.Inventory.TryPeek(ItemId.Animal, out item);
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

        public void AddVolunteer(Volunteer volunteer)
        {
            volunteer.UpdateQueuePlace(_queue.TakeQueue());
            _volunteersInQueue++;
            SetVolunteer(volunteer);
        }

        [Button("New Volunteer", enabledMode: EButtonEnableMode.Playmode)]
        public void CreateNewVolunteer()
        {
            for (int i = 0; i < _volunteers.Count; i++)
            {
                Volunteer volunteer = _volunteers[i];
                
                if (volunteer.IsFree)
                {
                    AddVolunteer(volunteer);
                    return;
                }
            }
            
            Volunteer newVolunteer = _gameFactory.CreateVolunteer(_spawnPlace.position, _container)
                .GetComponent<Volunteer>();
            AddVolunteer(newVolunteer);
            VolunteerStateMachine machine = newVolunteer.StateMachine;
            machine.Construct(_queue, _outPlace, newVolunteer);
            machine.Play();
        }

        private void SetVolunteer(Volunteer volunteer)
        {
            HandItem animal = _animalSpawner.SpawnAnimal(volunteer.transform.position);
            volunteer.Inventory.Add(animal);
            _volunteers.Add(volunteer);
        }

        [Button("Move Queue")]
        private void MoveQueue()
        {
            _queue.Move();
            _queue.FreeQueue();
        }
    }
}