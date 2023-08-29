using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AYellowpaper;
using Logic.Translators;
using NaughtyAttributes;
using NTC.Global.System;
using Tools.Extension;
using UnityEngine;

namespace Logic.TransformGrid
{
    [RequireComponent(typeof(RunTranslator))]
    public class FlexGrid : MonoBehaviour, ITransformGrid
    {
#if UNITY_EDITOR
        private const float DebugRadius = 0.5f;
#endif
        
        private readonly List<Transform> _cells = new List<Transform>();
        private readonly List<Vector3> _positions = new List<Vector3>();

        [SerializeField] private InterfaceReference<ITranslator, MonoBehaviour> _translator;
        [SerializeField] private GridAlign _align;
        [SerializeField] private float _space;
        
        [SerializeField] [Range(1, 100)] private int _horizontalClamp;
        [SerializeField] [Range(1, 100)] private int _verticalClamp;

#if UNITY_EDITOR
        [SerializeField] private GameObject _testPrefab;
#endif
        
        private float _elapsedTime;
        
        private int MaxSize => _horizontalClamp * _verticalClamp;

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            foreach (var position in _positions)
                Gizmos.DrawWireSphere(ToGlobalPosition(position), DebugRadius);
        }

        private Vector3 ToGlobalPosition(Vector3 position)
        {
            Transform selfTransform = transform;
            return selfTransform.TransformVector(position) + selfTransform.position;
        }

        public void AddCell(Transform cellTransform)
        {
            if (_cells.Count >= MaxSize)
                throw new ArgumentOutOfRangeException(nameof(_cells.Count));

            cellTransform.gameObject.Enable();
            cellTransform.SetParent(transform, true);
            _cells.Add(cellTransform);
            _positions.Add(cellTransform.localPosition);
            
            UpdatePositions();
            cellTransform.localPosition = _positions.Last();
            MoveCells();
            CustomScaleTranslatable customScaleTranslatable = cellTransform.GetComponent<CustomScaleTranslatable>();
            Translate(customScaleTranslatable, Vector3.zero, Vector3.one);
        }

        public void RemoveCell(Transform cellTransform)
        {
            if (_cells.Count <= 0)
                throw new ArgumentOutOfRangeException(nameof(_cells.Count));

            cellTransform.Unparent();
            _cells.Remove(cellTransform);
            _positions.Remove(_positions.Last());
            
            UpdatePositions();
            MoveCells();
            CustomScaleTranslatable translatable = cellTransform.GetComponent<CustomScaleTranslatable>();
            Translate(translatable, Vector3.one, Vector3.zero, cellTransform.gameObject.Disable);
        }

        public void RemoveAll()
        {
            int cellsCount = _cells.Count;
            
            for (int i = 0; i < cellsCount; i++)
            {
                RemoveCell(_cells[i]);
            }
        }
        
        [Button] [Conditional("UNITY_EDITOR")]
        private void ManualAdd()
        {
            AddCell(Instantiate(_testPrefab).transform);
        }
        
        [Button] [Conditional("UNITY_EDITOR")]
        private void ManualRemove()
        {
            RemoveCell(_cells.GetRandom());
        }

        private void MoveCells()
        {
            for (var index = 0; index < _cells.Count; index++)
            {
                Transform cell = _cells[index];

                Vector3 nextPosition = _positions[index];
                if (cell.localPosition.Equals(nextPosition))
                    continue;
                
                Translate(cell.GetComponent<CustomPositionTranslatable>(),  cell.localPosition, nextPosition);
            }
        }

        private void UpdatePositions()
        {
            Vector2 origin = Vector2.zero;

            int cellCount = _cells.Count;
            int maxHorizontal = Mathf.Min(_horizontalClamp, cellCount);
            int maxVertical = Mathf.Min(_verticalClamp, Mathf.CeilToInt(cellCount / (float) maxHorizontal));

            HorizontalAlign horizontalAlign = _align.IsolateHorizontal();
            VerticalAlign verticalAlign = _align.IsolateVertical();

            for (int vertical = 0; vertical < maxVertical; vertical++)
            {
                float verticalPosition = 0;
                int maxHorizontalInRaw = Mathf.Min(maxHorizontal, cellCount - vertical * maxHorizontal);

                switch (verticalAlign)
                {
                    case VerticalAlign.Top:
                        verticalPosition = origin.y - _space * vertical;
                        break;
                    case VerticalAlign.Center:
                        verticalPosition = origin.y + _space * vertical - _space * (maxVertical - 1) / 2f;
                        break;
                    case VerticalAlign.Bottom:
                        verticalPosition = origin.y + _space * vertical;
                        break;
                }
                
                for (int horizontal = 0; horizontal < maxHorizontalInRaw; horizontal++)
                {
                    int positionIndex = horizontal + vertical * maxHorizontal;

                    Vector2 position = Vector2.zero;
                    
                    switch (horizontalAlign)
                    {
                        case HorizontalAlign.Left:
                            position = new Vector2(origin.x + _space * horizontal, verticalPosition);
                            break;
                        case HorizontalAlign.Center:
                            position = new Vector2(origin.x + _space * horizontal - _space * (maxHorizontalInRaw - 1) / 2f, verticalPosition);
                            break;
                        case HorizontalAlign.Right:
                            position = new Vector2(origin.x - _space * horizontal, verticalPosition);
                            break;
                    }

                    _positions[positionIndex] = position;
                }
            }
        }

        private void Translate(ITranslatableParametric<Vector3> translatable, Vector3 from, Vector3 to, Action onEndCallback)
        {
            translatable.End += OnEndTranslate;
            Translate(translatable, from, to);

            void OnEndTranslate(ITranslatable _)
            {
                onEndCallback.Invoke();
                translatable.End -= OnEndTranslate;
            }
        }
        
        private void Translate(ITranslatableParametric<Vector3> translatable, Vector3 from, Vector3 to)
        {
            translatable.Play(from, to);
            _translator.Value.AddTranslatable(translatable);
        }
    }
}