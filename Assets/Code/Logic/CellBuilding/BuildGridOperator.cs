using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using NaughtyAttributes;
using NTC.Global.System;
using Services;
using Tutorial;
using UnityEngine;

namespace Logic.CellBuilding
{
    public abstract class BuildGridOperator : MonoBehaviour, ITutorialTrigger
    {
        private const string MaxCountException = "Trying to build more than the maximum number of buildings";

        private readonly Queue<BuildPlaceMarker> _positions = new Queue<BuildPlaceMarker>();

        [SerializeField] private List<BuildPlaceMarker> _buildPlaces;
        [SerializeField] private int _buildCost;

        protected IGameFactory GameFactory;
        protected BuildCell ActiveBuildCell;
        
        private BuildPlaceMarker _currentMarker;

        public event Action Triggered = () => { };

        private void Awake()
        {
            FillPositions();
            GameFactory = AllServices.Container.Single<IGameFactory>();
            _currentMarker = _positions.Dequeue();
            ActiveBuildCell = GameFactory.CreateBuildCell(_currentMarker.Location.Position, _currentMarker.Location.Rotation)
                .GetComponent<BuildCell>();
            ActiveBuildCell.SetBuildCost(_buildCost);
            ActiveBuildCell.SetIcon(_currentMarker.Icon);
            ActiveBuildCell.Build += ActivateNext;
            OnAwake();
        }

        private void OnDestroy() =>
            ActiveBuildCell.Build -= ActivateNext;

        protected abstract void BuildCell(BuildPlaceMarker marker);

        protected virtual void OnAwake()
        {
        }

        private void FillPositions()
        {
            for (var index = 0; index < _buildPlaces.Count; index++)
            {
                BuildPlaceMarker marker = _buildPlaces[index];
                marker.Init();
                _positions.Enqueue(marker);
            }
        }

        [Button("Activate Next")]
        private void ActivateNext()
        {
            BuildCell(_currentMarker);
            Triggered.Invoke();
            
            if (_positions.TryDequeue(out BuildPlaceMarker marker))
            {
                ActiveBuildCell.Reposition(marker.Location);
                ActiveBuildCell.SetIcon(marker.Icon);
                _currentMarker = marker;
                return;
            }
            
            ActiveBuildCell.gameObject.Disable();
        }
    }
}