using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Movement
{
    public class TowardsMover : MonoBehaviour, IMover
    {
        public event UnityAction StartMove;
        public event UnityAction GotToPlace;
        [HideInInspector] public bool IsMoving = false;

        [SerializeField, Min(.0f)] private float _speed = 5.0f;
        [SerializeField, Min(.0f)] private float _offset = 0.1f;

        private Transform target = null;
        private Vector3 lastPos = Vector3.zero;

        public void Move(Transform target)
        {
            this.target = target;
            StartCoroutine(MoveCor());
        }

        public IEnumerator MoveCor()
        {
            IsMoving = true;
            StartMove?.Invoke();
            while ((transform.position - target.position).magnitude > _offset
                   && lastPos != transform.position)
            {
                lastPos = transform.position;
                transform.position = Vector3.MoveTowards(transform.position,
                    target.position,
                    _speed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
            IsMoving = false;
            GotToPlace?.Invoke();
        }

    }
}
