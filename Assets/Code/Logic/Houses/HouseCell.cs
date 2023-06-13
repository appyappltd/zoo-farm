using System;
using Logic.Interactions;
using Tools.Extension;
using UnityEngine;

namespace Logic.Houses
{
    [RequireComponent(typeof(TriggerObserver))]
    public class HouseCell : MonoBehaviour
    {
        [SerializeField] private int _buildCost;
        
        private TriggerObserver _triggerObserver;
        
        public event Action BuildHouse = () => { };

        private void Awake() =>
            _triggerObserver = GetComponent<TriggerObserver>();

        private void OnEnable() =>
            _triggerObserver.Enter += OnEnter;

        private void OnDisable() =>
            _triggerObserver.Enter -= OnEnter;

        private void OnEnter(GameObject obj)
        {
            if (obj.TryGetComponent(out Wallet.Wallet wallet) == false)
                return;
            
            bool hasMoney = wallet.TrySpend(_buildCost);

            if (hasMoney)
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