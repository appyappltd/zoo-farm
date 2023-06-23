using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Movement
{
    public interface IMover
    {
        public event UnityAction StartMove;
        public event UnityAction GotToPlace;

        public void Move(Transform target);
        public IEnumerator MoveCor();
    }
}
