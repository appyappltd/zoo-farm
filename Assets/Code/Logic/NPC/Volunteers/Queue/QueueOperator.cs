using System;
using UnityEngine;

namespace Logic.NPC.Volunteers.Queue
{
    public class QueueOperator
    {
        private readonly QueuePlace[] _places;

        private int _freeIndex;

        public int QueueCount => _freeIndex;
        
        public QueuePlace Head => _places[0];
        public QueuePlace Tail => _places[^1];

        public QueueOperator(QueuePlace[] places)
        {
            _places = places;
            
            for (int i = 0; i < _places.Length; i++)
            {
                QueuePlace queuePlace = _places[i];
                queuePlace.Vacated += OnVacatedPlace;
                queuePlace.Hidden += OnHiddenPlace;
            }
        }

        public QueuePlace TakeQueue(Volunteer volunteer)
        {
            _freeIndex++;
            
            if (_freeIndex > _places.Length)
                throw new IndexOutOfRangeException();

            QueuePlace queuePlace = GetFreeQueuePlace();
            queuePlace.Construct(volunteer.Inventory);
            return queuePlace;
        }

        private void OnHiddenPlace(QueuePlace hidden)
        {
            int vacateIndex = FindVacateIndex(hidden);
            Move(vacateIndex);
            FreeQueue();
        }

        private void OnVacatedPlace(QueuePlace vacated)
        {
            for (int i = FindVacateIndex(vacated); i < _places.Length; i++)
            {
                _places[i].Hide();
            }
        }

        private void Move(int fromIndex)
        {
            if (IsLastIndex(fromIndex))
                return;

            Vector3 lastPosition = GetPositionOfLastPlace();

            MoveQueueTransforms();

            _places[fromIndex].transform.position = lastPosition;
            QueuePlace firstPlace = _places[fromIndex];
            
            MoveQueueInArray(fromIndex);

            _places[^1] = firstPlace;
        }

        private void MoveQueueInArray(int fromIndex)
        {
            for (int i = fromIndex + 1; i < _places.Length; i++)
            {
                _places[i - 1] = _places[i];
            }
        }

        private void MoveQueueTransforms()
        {
            for (int i = _places.Length - 1; i > 0; i--)
                _places[i].transform.position = _places[i - 1].transform.position;
        }

        private Vector3 GetPositionOfLastPlace() =>
            _places[^1].transform.position;

        private bool IsLastIndex(int fromIndex) =>
            fromIndex == _places.Length - 1;

        private QueuePlace GetFreeQueuePlace() =>
            _places[_freeIndex - 1];

        private int FindVacateIndex(QueuePlace vacated) =>
            Array.FindIndex(_places, place => Equals(place, vacated));

        private void FreeQueue() =>
            _freeIndex--;
    }
}