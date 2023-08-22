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
        private readonly List<Transform> _cells = new List<Transform>();
        private readonly List<Vector3> _positions = new List<Vector3>();

        [SerializeField] private InterfaceReference<ITranslator, MonoBehaviour> _translator;
        [SerializeField] private GridAlign _align;
        [SerializeField] private float _space;
        
        [SerializeField] [Range(1, 100)] private int _horizontalClamp;
        [SerializeField] [Range(1, 100)] private int _verticalClamp;

        private float _elapsedTime;
        
        private int MaxSize => _horizontalClamp * _verticalClamp;

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            foreach (var position in _positions)
                Gizmos.DrawWireCube(position, Vector3.one * 0.5f);
        }

        public void AddCell(Transform transform)
        {
            if (_cells.Count >= MaxSize)
                throw new ArgumentOutOfRangeException(nameof(_cells.Count));

            transform.gameObject.Enable();
            _cells.Add(transform);
            _positions.Add(transform.position);
            
            UpdatePositions();
            transform.position = _positions.Last();
            MoveCells();
            Translate(transform.GetComponent<CustomScaleTranslatable>(), Vector3.zero, Vector3.one);
        }

        public void RemoveCell(Transform transform)
        {
            if (_cells.Count <= 0)
                throw new ArgumentOutOfRangeException(nameof(_cells.Count));

            _cells.Remove(transform);
            _positions.Remove(_positions.Last());
            
            UpdatePositions();
            MoveCells();
            CustomScaleTranslatable translatable = transform.GetComponent<CustomScaleTranslatable>();
            Translate(translatable, Vector3.one, Vector3.zero, transform.gameObject.Disable);
        }

        [Button] [Conditional("UNITY_EDITOR")]
        private void ManualAdd()
        {
            AddCell(new GameObject("TestGrid").transform);
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
                if (cell.position.Equals(nextPosition))
                    continue;
                
                Translate(cell.GetComponent<CustomPositionTranslatable>(), cell.position, nextPosition);
            }
        }

        private void UpdatePositions()
        {
            Vector3 transformPosition = transform.position;
            float originY = transformPosition.y;
            Vector2 origin = transformPosition.RemoveY();

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

                    _positions[positionIndex] = position.AddY(originY);
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