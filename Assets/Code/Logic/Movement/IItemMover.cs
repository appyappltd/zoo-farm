using System;
using UnityEngine;

namespace Logic.Movement
{
    public interface IItemMover
    {
        public event Action Started;
        public event Action Ended;
        public void Move(Transform to, Transform finishParent = null, bool isModifyRotation = false, bool isModifyScale = false);
        public void Move(Transform to, Action OnMoveEnded, Transform finishParent = null, bool isModifyRotation = false,  bool isModifyScale = false);
    }
}
