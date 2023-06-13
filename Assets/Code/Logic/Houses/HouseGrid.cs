using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using NaughtyAttributes;
using Services;
using Services.AnimalHouse;
using Tools.Constants;
using Tools.Extension;
using UnityEngine;

namespace Logic.Houses
{
    public class HouseGrid : MonoBehaviour
    {
        private const string MaxHouseCountException = "Trying to build more than the maximum number of houses";

        private readonly Queue<Vector3> _housePositions = new Queue<Vector3>();

        [SerializeField] private Vector2Int _gridSize;
        [SerializeField] private Vector2 _cellOffset;
        [SerializeField] private Vector2 _cellSize;

        private IAnimalHouseService _houseService;
        private HouseCell _activeHouseCell;
        private IGameFactory _gameFactory;

        private void Awake()
        {
            _houseService = AllServices.Container.Single<IAnimalHouseService>();
            _gameFactory = AllServices.Container.Single<IGameFactory>();
            FillPositions();
            _activeHouseCell = _gameFactory.CreateHouseCell(_housePositions.Peek()).GetComponent<HouseCell>();
            _activeHouseCell.BuildHouse += ActivateNext;
        }

        private void OnDestroy()
        {
            _activeHouseCell.BuildHouse -= ActivateNext;
        }

        private void OnDrawGizmos()
        {
            foreach (Vector3 position in GetPositions())
            {
                Gizmos.DrawCube(position.ChangeY(transform.position.y + _cellSize.x * Arithmetic.ToHalf), _cellSize.AddY(_cellSize.x));
            }
        }

        private void FillPositions()
        {
            List<Vector3> list = GetPositions();
            
            for (var index = 0; index < list.Count; index++)
            {
                var position = list[index];
                _housePositions.Enqueue(position);
            }
        }

        private List<Vector3> GetPositions()
        {
            List<Vector3> positions = new List<Vector3>();

            Vector2 gridLength = new Vector2(
                _gridSize.x * (_cellSize.x + _cellOffset.x) - _cellOffset.x,
                _gridSize.y * (_cellSize.y + _cellOffset.y) - _cellOffset.y);

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    Vector2 boxPositionAbsolute = new Vector2(
                        x * (_cellSize.x + _cellOffset.x),
                        y * (_cellSize.y + _cellOffset.y));


                    Vector3 selfPosition = transform.position;
                    Vector3 boxPositionRelative = 
                        selfPosition +
                        (boxPositionAbsolute - (gridLength - _cellSize) * Arithmetic.ToHalf)
                        .AddY(selfPosition.y);
                    
                    positions.Add(boxPositionRelative);
                }
            }

            return positions;
        }

        [Button("Activate Next")]
        private void ActivateNext()
        {
            if (_housePositions.TryDequeue(out Vector3 nextPosition))
            {
                _houseService.BuildHouse(nextPosition);
                _activeHouseCell.Reposition(nextPosition);
                return;
            }

            throw new Exception(MaxHouseCountException);
        }
    }
}