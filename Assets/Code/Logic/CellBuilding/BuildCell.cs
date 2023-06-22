using System;
using Logic.Wallet;
using Tools.Extension;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class BuildCell : MonoBehaviour
    {
        [SerializeField] private Consumer _consumer;

        public int BuildCost { get; private set; }

        public event Action Build = () => { };

        private void OnEnable() =>
            _consumer.Bought += OnEnter;

        private void OnDisable() =>
            _consumer.Bought -= OnEnter;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position.ChangeY(transform.position.y + 0.5f), Vector3.one);
        }

        public void Reposition(Vector3 nextPosition)
        {
            transform.position = nextPosition;
        }

        public void SetBuildCost(int buildCost)
        {
            BuildCost = buildCost;
            _consumer.SetCost(buildCost);
        }

        private void OnEnter()
        {
            Build.Invoke();
        }
    }
}
