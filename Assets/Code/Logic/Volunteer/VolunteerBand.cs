using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using Logic.Inventory;
using Logic.Movement;
using Tools.Extension;
using UnityEngine;
using UnityEngine.UIElements;

namespace Logic.Volunteer
{
    public class VolunteerBand : MonoBehaviour
    {
        [SerializeField, Min(.0f)] private float _distance = 1.5f;
        [SerializeField] private Transform _defTarget;
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _defTargetOut1;
        [SerializeField] private Transform _defTargetOut2;
        [SerializeField] private VolunteerSpawner _spawner;
        [SerializeField] private TransmittingAnimals _transmitting;

        private List<VolunteerMovement> volunteers = new();
        private Rotater rotater;
        private AnimationMover mover;

        private void Awake()
        {
            _target.position = _defTarget.position;
            _spawner.SpawnV += AddVolunteer;
        }

        public void AddVolunteer(VolunteerMovement volunteer)
        {
            volunteers.Add(volunteer);
            mover = volunteer.GetComponent<AnimationMover>();
            rotater = volunteer.GetComponent<Rotater>();
            volunteer.GetComponent<PathMover>().SetPoints(new Transform[] { _defTargetOut1, _defTargetOut2 });

            rotater.Rotate(_target);
            mover.Move(_target);
            mover.GotToPlace += RotateVolunteer;
            mover.GotToPlace += Wait;

            UpdateTarget();
        }
        private void Wait()
        {
            mover.GotToPlace -= Wait;
            _spawner.isReady = true;
            _spawner.TrySpawn();
        }

        private void RotateVolunteer()
        {
            rotater.Rotate(_defTarget);
        }

        public bool CanGiveAnimal() => volunteers.Count > 0 && !volunteers.First().GetComponent<AnimationMover>().IsMoving;

        public HandItem GetAnimal()
        {
            var volunteer = volunteers.First();
            var inventory = volunteer.GetComponent<Inventory.Inventory>();
            var animal = inventory.Remove();

            RemoveVolunteer();

            return animal;
        }

        private void RemoveVolunteer()
        {
            var volunteer = volunteers.First();
            volunteer.GetComponent<PathMover>().StartWalk();
            rotater = volunteer.GetComponent<Rotater>();
            volunteer.GetComponent<AnimationMover>().GotToPlace -= RotateVolunteer;

            volunteers.Remove(volunteer);
            MoveQueue();
        }

        private void UpdateTarget()
            => _target.position = _target.position.ChangeX(_target.position.x + _distance);

        private void MoveQueue()
        {
            var newVs = new List<VolunteerMovement>(volunteers);
            volunteers.Clear();
            _target.position = _defTarget.position;

            foreach (var v in newVs)
                AddVolunteer(v);
        }
    }
}
