using System.Diagnostics;
using Infrastructure.Factory;
using Logic.Payment;
using NaughtyAttributes;
using NTC.Global.System;
using Services;
using UnityEngine;

namespace Logic.CellBuilding
{
    public abstract class BuildGridOperator : MonoBehaviour
    {
        [SerializeField] private BuildPlaceMarker[] _buildPlaces;
        [SerializeField] private int[] _buildCosts;
        [SerializeField] private Vector3 _buildOffset;
        [SerializeField] private bool _isAutoBuild = true;
        [SerializeField] private ConsumeType _consumeType = ConsumeType.Smooth;

        protected IGameFactory GameFactory;
        
        private BuildCell _activeBuildCell;
        private int _currentIndex;
        private BuildPlaceMarker _currentMarker;
        private bool _isCellBuilt = true;

        public Vector3 BuildCellPosition => _activeBuildCell.transform.position;

        private void Awake()
        {
            InitMarkers();
            GameFactory = AllServices.Container.Single<IGameFactory>();
            _currentMarker = _buildPlaces[0];
            _activeBuildCell = GameFactory.CreateBuildCell(_currentMarker.Location.Position, _currentMarker.Location.Rotation, _consumeType)
                .GetComponent<BuildCell>();
            _activeBuildCell.transform.SetParent(transform, true);
            _activeBuildCell.gameObject.SetActive(false);
            _activeBuildCell.SetBuildCost(_buildCosts[0]);
            _activeBuildCell.SetIcon(_currentMarker.Icon);
            _activeBuildCell.Build += BuildAndActivate;

            if (_isAutoBuild)
                ShowNextBuildCell();

            OnAwake();
        }

        private void OnDestroy() =>
            _activeBuildCell.Build -= BuildAndActivate;

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            ApplyOffsetToMarkers();
        }

        protected abstract void BuildCell(BuildPlaceMarker marker);

        protected virtual void OnAwake() { }

        public void Enable()
        {
            gameObject.SetActive(true);
            
            if (_isAutoBuild)
                BuildAndActivate();
        }

        public void Disable() =>
            gameObject.SetActive(false);

        public void SetAutoNext(bool isAuto) =>
            _isAutoBuild = isAuto;

        [Button("Show Next")]
        public void ShowNextBuildCell()
        {
            if (_isCellBuilt == false)
                return;

            if (_currentIndex >= _buildPlaces.Length)
                return;

            _currentMarker = _buildPlaces[_currentIndex];
            _activeBuildCell.Reposition(_currentMarker.Location);
            _activeBuildCell.SetIcon(_currentMarker.Icon);
            _activeBuildCell.SetBuildCost(_buildCosts[_currentIndex]);
            _isCellBuilt = false;
            _activeBuildCell.gameObject.Enable();
            _currentIndex++;
        }

        private void BuildAndActivate()
        {
            BuildCell();
            
            if (_isAutoBuild)
                ShowNextBuildCell();
        }

        private void BuildCell()
        {
            BuildCell(_currentMarker);
            _isCellBuilt = true;
            _activeBuildCell.gameObject.Disable();
        }

        private void InitMarkers()
        {
            for (int i = 0; i < _buildPlaces.Length; i++)
                _buildPlaces[i].Init(_buildOffset);
        }

        [Button("CollectMarkers", EButtonEnableMode.Editor)] [Conditional("UNITY_EDITOR")]
        private void CollectMarkers() =>
            _buildPlaces = GetComponentsInChildren<BuildPlaceMarker>();
        
        [Button("ApplyOffsetToMarkers", EButtonEnableMode.Editor)] [Conditional("UNITY_EDITOR")]
        private void ApplyOffsetToMarkers()
        {
            foreach (var place in _buildPlaces)
                place.Init(_buildOffset);
        }
    }
}
