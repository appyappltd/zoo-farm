using System;
using Logic.Interactions;
using Tools.Extension;
using UnityEngine;

namespace Logic.Houses
{
    [RequireComponent(typeof(TriggerObserver))]
    public class HouseCell : MonoBehaviour
    {
        private Consumer _consumer;

        public event Action BuildHouse = () => { };

        private void Awake() =>
            _consumer = GetComponent<Consumer>();

        private void OnEnable() =>
            _consumer.Bought += OnEnter;

        private void OnDisable() =>
            _consumer.Bought -= OnEnter;

        private void OnEnter()
        {
            BuildHouse.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position.ChangeY(transform.position.y + 0.5f), Vector3.one);
        }

        public void Reposition(Vector3 nextPosition)
        {
            transform.position = nextPosition;
        }
    }
}
