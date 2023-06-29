using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using NaughtyAttributes;
using NTC.Global.System;
using Services;
using Tools.Extension;
using Tutorial;
using UnityEngine;

namespace Logic.CellBuilding
{
    public abstract class BuildGridOperator : MonoBehaviour, ITutorialTrigger
    {
        private const string MaxCountException = "Trying to build more than the maximum number of buildings";

        private readonly Queue<Location> _positions = new Queue<Location>();

        [SerializeField] private List<Transform> _buildPlaces;
        [SerializeField] private int _buildCost;

        protected IGameFactory GameFactory;

        private BuildCell _activeBuildCell;

        public event Action Triggered = () => { };

        private void Awake()
        {
            FillPositions();
            GameFactory = AllServices.Container.Single<IGameFactory>();
            Location location = _positions.Dequeue();
            _activeBuildCell = GameFactory.CreateBuildCell(location.Position, location.Rotation).GetComponent<BuildCell>();
            _activeBuildCell.SetBuildCost(_buildCost);
            _activeBuildCell.Build += ActivateNext;
            OnAwake();
        }

        private void OnDestroy() =>
            _activeBuildCell.Build -= ActivateNext;

        private void OnDrawGizmos()
        {
            foreach (Transform place in _buildPlaces)
            {
                Gizmos.DrawCube( place.position.ChangeY(place.position.y + 0.5f), Vector3.one);
                Vector3 forwardMarkedPosition = place.position + place.forward * 0.8f;
                Gizmos.DrawSphere( forwardMarkedPosition.ChangeY(forwardMarkedPosition.y + 0.5f), 0.25f);
            }
        }

        protected abstract void BuildCell(Vector3 position, Quaternion rotation);

        protected virtual void OnAwake()
        {
        }

        private void FillPositions()
        {
            for (var index = 0; index < _buildPlaces.Count; index++)
            {
                Transform place = _buildPlaces[index];
                _positions.Enqueue(new Location(place.position, place.rotation));
            }
        }

        [Button("Activate Next")]
        private void ActivateNext()
        {
            Transform cellTransform = _activeBuildCell.transform;
            BuildCell(cellTransform.position, cellTransform.rotation);
            Triggered.Invoke();
            
            if (_positions.TryDequeue(out Location location))
            {
                _activeBuildCell.Reposition(location);
                return;
            }
            
            _activeBuildCell.gameObject.Disable();
        }
    }
}