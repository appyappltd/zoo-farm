using System;
using System.Collections;
using UnityEngine;

namespace Logic.Movement
{
    public class AnimationItemMover : MonoBehaviour, IItemMover
    {
        [HideInInspector] public bool IsMoving = false;
        [SerializeField, Min(.0f)] private float _offset = 0.1f;

        private Vector3 _targetPosition;
        public event Action Started;
        public event Action Ended;

        public void Move(Transform to, Transform finishParent)
        {
            _targetPosition = to.position;
            StartCoroutine(MoveCor());
        }

        public void Move(Transform to, Action OnMoveEnded, Transform finishParent = null)
        {
           Move(to, finishParent);
        }

        public IEnumerator MoveCor()
        {
            IsMoving = true;
            Started?.Invoke();
            while ((transform.position - _targetPosition).magnitude > _offset)
                yield return new WaitForFixedUpdate();
            IsMoving = false;
            Ended?.Invoke();
        }
    }
}
