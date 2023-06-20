using System.Collections.Generic;
using Infrastructure.Factory;
using NaughtyAttributes;
using NTC.Global.System;
using Services;
using Tools.Extension;
using UnityEngine;

namespace Logic.CellBuilding
{
    public abstract class BuildGridOperator : MonoBehaviour
    {
        private const string MaxHouseCountException = "Trying to build more than the maximum number of buildings";

        private readonly Queue<Vector3> _housePositions = new Queue<Vector3>();

        [SerializeField] private List<Transform> _buildPlaces;
        [SerializeField] private int _buildCost;

        protected IGameFactory GameFactory;

        private BuildCell _activeBuildCell;

        private void Awake()
        {
            FillPositions();
            GameFactory = AllServices.Container.Single<IGameFactory>();
            _activeBuildCell = GameFactory.CreateBuildCell(_housePositions.Dequeue()).GetComponent<BuildCell>();
            _activeBuildCell.SetBuildCost(_buildCost);
            _activeBuildCell.Build += ActivateNext;
            OnAwake();
        }

        private void OnDestroy() =>
            _activeBuildCell.Build -= ActivateNext;

        private void OnDrawGizmos()
        {
            foreach (Vector3 position in _housePositions)
            {
                Gizmos.DrawCube(position.ChangeY(position.y + 0.5f), Vector3.one);
            }
        }

        protected abstract void BuildCell(Vector3 position);

        protected virtual void OnAwake()
        {
        }

        private void FillPositions()
        {
            for (var index = 0; index < _buildPlaces.Count; index++)
            {
                var place = _buildPlaces[index];
                _housePositions.Enqueue(place.position);
            }
        }

        [Button("Activate Next")]
        private void ActivateNext()
        {
            BuildCell(_activeBuildCell.transform.position);
            
            if (_housePositions.TryDequeue(out Vector3 nextPosition))
            {
                _activeBuildCell.Reposition(nextPosition);
                return;
            }
            
            _activeBuildCell.gameObject.Disable();
        }
    }
}