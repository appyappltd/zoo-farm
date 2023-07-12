using System;
using UnityEngine;

namespace Logic.Volunteers
{
    public class Queue
    {
        private readonly Transform[] _places;

        private int _freeIndex;
        
        public Transform Head => _places[0];
        public Transform Tail => _places[^1];

        public Queue(Transform[] places)
        {
            _places = places;

            for (int i = 1; i < _places.Length; i++)
            {
                _places[i].LookAt(_places[i - 1]);
            }
        }

        public void Move()
        {
            Vector3 lastPosition = _places[^1].position;

            for (int i = _places.Length - 1; i > 0; i--)
            {
                _places[i].position = _places[i - 1].position;
            }

            _places[0].position = lastPosition;

            Transform firstPlace = _places[0];
            
            for (int i = 1; i < _places.Length; i++)
            {
                _places[i - 1] = _places[i];
            }

            _places[^1] = firstPlace;
        }

        public Transform TakeQueue()
        {
            _freeIndex++;
            
            if (_freeIndex > _places.Length)
            { 
                throw new IndexOutOfRangeException();
            }

            return _places[_freeIndex - 1];
        }
        
        public void FreeQueue()
        {
            _freeIndex--;
            
            if (_freeIndex < 0)
            {
                throw new IndexOutOfRangeException();
            }
        }
    }
}