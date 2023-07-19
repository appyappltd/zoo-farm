using System.Collections.Generic;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Volunteers.Queue;
using Logic.VolunteersStateMachine;
using NaughtyAttributes;
using Services;
using UnityEngine;

namespace Logic.Volunteers
{
    public class VolunteerBand : MonoBehaviour
    {
        [SerializeField] private AnimalSpawner _animalSpawner;

        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private Transform _outPlace;

        [SerializeField] private QueuePlace[] _queuePlaces;
        [SerializeField] private List<Volunteer> _volunteers = new();

        private IGameFactory _gameFactory;
        private QueueOperator _queueOperator;
        private Volunteer _volunteerCashed;

        public int VolunteersCount => _queueOperator.QueueCount;
        public int MaxVolunteers => _queuePlaces.Length - 2;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _queueOperator = new QueueOperator(_queuePlaces);
        }

        private void SetVolunteer(Volunteer volunteer)
        {
            volunteer.UpdateQueuePlace(_queueOperator.TakeQueue(volunteer));
            HandItem animal = _animalSpawner.SpawnAnimal(volunteer.transform.position);
            volunteer.Inventory.Add(animal);
            _volunteers.Add(volunteer);
        }

        [Button("New Volunteer", enabledMode: EButtonEnableMode.Playmode)]
        public void CreateNewVolunteer()
        {
            for (int i = 0; i < _volunteers.Count; i++)
            {
                Volunteer volunteer = _volunteers[i];
                
                if (volunteer.IsFree)
                {
                    SetVolunteer(volunteer);
                    return;
                }
            }
            
            Volunteer newVolunteer = _gameFactory.CreateVolunteer(_spawnPlace.position, _container)
                .GetComponent<Volunteer>();
            SetVolunteer(newVolunteer);
            VolunteerStateMachine machine = newVolunteer.StateMachine;
            machine.Construct(_queueOperator, _outPlace, newVolunteer);
            machine.Play();
        }
    }
}