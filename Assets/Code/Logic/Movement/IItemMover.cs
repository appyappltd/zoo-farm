using System;
using UnityEngine;

namespace Logic.Movement
{
    public interface IItemMover
    {
        public event Action Started;
        public event Action Ended;

        public void Move(Transform to, Transform finishParent = null);
        
        public void Move(Transform to, Action OnMoveEnded, Transform finishParent = null);
    }
}
