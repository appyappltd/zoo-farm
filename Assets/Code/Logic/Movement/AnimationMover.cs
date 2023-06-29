using System;
using System.Collections;
using UnityEngine;

namespace Logic.Movement
{
    public class AnimationMover : MonoBehaviour, IMover
    {
        public event Action StartMove;
        public event Action GotToPlace;
        [HideInInspector] public bool IsMoving = false;

        [SerializeField, Min(.0f)] private float _offset = 0.1f;

        private Vector3 _targetPosition;

        public void Move(Transform target)
        {
            _targetPosition = target.position;
            StartCoroutine(MoveCor());
        }

        public IEnumerator MoveCor()
        {
            IsMoving = true;
            StartMove?.Invoke();
            while ((transform.position - _targetPosition).magnitude > _offset)
                yield return new WaitForFixedUpdate();
            IsMoving = false;
            GotToPlace?.Invoke();
        }
    }
}
