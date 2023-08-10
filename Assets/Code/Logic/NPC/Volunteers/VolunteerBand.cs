using System.Collections.Generic;
using Logic.Animals;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.NPC.Volunteers.Queue;
using Logic.NPC.Volunteers.VolunteersStateMachine;
using NaughtyAttributes;
using Services;
using UnityEngine;

namespace Logic.NPC.Volunteers
{
    public class VolunteerBand : MonoBehaviour
    {
        [SerializeField] private AnimalItemSpawner _animalItemSpawner;

        [SerializeField] private Transform _container;
        [SerializeField] private Transform _spawnPlace;
        [SerializeField] private Transform _outPlace;

        [SerializeField] private QueuePlace[] _queuePlaces;

        private IGameFactory _gameFactory;
        private QueueOperator _queueOperator;
        private Volunteer _volunteerCashed;
        private List<Volunteer> _volunteers;

        public int VolunteersCount => _queueOperator.QueueCount;
        public int MaxVolunteers => _queuePlaces.Length - 2;

        private void Awake()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            _queueOperator = new QueueOperator(_queuePlaces);
            _volunteers = new List<Volunteer>(_queuePlaces.Length + 1);
        }

        private void SetVolunteer(Volunteer volunteer)
        {
            volunteer.UpdateQueuePlace(_queueOperator.TakeQueue(volunteer));
            HandItem animal = _animalItemSpawner.SpawnAnimal(volunteer.transform.position);
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
            machine.Construct(_outPlace, newVolunteer);
            machine.Play();
        }
    }
}