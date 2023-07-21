using System;
using UnityEngine;

namespace Logic.Volunteers.Queue
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
            _places[0].Vacated += OnVacatedPlace;
            
            for (int i = 1; i < _places.Length; i++)
            {
                QueuePlace queuePlace = _places[i];
                queuePlace.Vacated += OnVacatedPlace;
            }
        }

        public QueuePlace TakeQueue(Volunteer volunteer)
        {
            _freeIndex++;
            
            if (_freeIndex > _places.Length)
            { 
                throw new IndexOutOfRangeException();
            }

            QueuePlace queuePlace = _places[_freeIndex - 1];
            queuePlace.Construct(volunteer.Inventory);
            return queuePlace;
        }

        private void OnVacatedPlace(QueuePlace vacated)
        {
            int vacateIndex = Array.FindIndex(_places, place => Equals(place, vacated));
            Move(vacateIndex);
            FreeQueue();
        }

        private void Move(int fromIndex)
        {
            if (fromIndex == _places.Length - 1)
                return;

            Vector3 lastPosition = _places[^1].transform.position;

            for (int i = _places.Length - 1; i > 0; i--)
            {
                _places[i].transform.position = _places[i - 1].transform.position;
            }

            _places[fromIndex].transform.position = lastPosition;

            QueuePlace firstPlace = _places[fromIndex];
            
            for (int i = fromIndex + 1; i < _places.Length; i++)
            {
                _places[i - 1] = _places[i];
            }

            _places[^1] = firstPlace;
        }

        private void FreeQueue()
        {
            _freeIndex--;
            
            // if (_freeIndex < 0)
            // {
            //     throw new IndexOutOfRangeException();
            // }
        }
    }
}