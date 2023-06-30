using System;
using System.Collections;
using UnityEngine;

namespace Logic.Movement
{
    public interface IMover
    {
        public event Action StartMove;
        public event Action GotToPlace;

        public void Move(Transform target);
        public IEnumerator MoveCor();
    }
}
