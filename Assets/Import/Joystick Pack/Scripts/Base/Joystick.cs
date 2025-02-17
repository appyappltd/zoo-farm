﻿using Services.Input;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Joystick_Pack.Scripts.Base
{
    public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IInputReader
    {
        public float Horizontal => snapX ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x;

        public float Vertical => snapY ? SnapFloat(input.y, AxisOptions.Vertical) : input.y;

        public Vector2 Direction => new(Horizontal, Vertical);

        public float HandleRange
        {
            get => handleRange;
            set => handleRange = Mathf.Abs(value);
        }

        public float DeadZone
        {
            get => deadZone;
            set => deadZone = Mathf.Abs(value);
        }

        public AxisOptions AxisOptions
        {
            get => AxisOptions;
            set => axisOptions = value;
        }

        public bool SnapX
        {
            get => snapX;
            set => snapX = value;
        }

        public bool SnapY
        {
            get => snapY;
            set => snapY = value;
        }

        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone;
        [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
        [SerializeField] private bool snapX;
        [SerializeField] private bool snapY;

        [SerializeField] protected RectTransform background;
        [SerializeField] private RectTransform handle;
        
        private RectTransform baseRect;
        private Canvas canvas;
        private Camera cam;
        private Vector2 input;

        public virtual void Init()
        {
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();
            if (canvas == null)
                Debug.LogError("The Joystick is not placed inside a canvas");

            var center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;
            input = Vector2.zero;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            var position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            var radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            handle.anchoredPosition = input * radius * handleRange;
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
            {
                input = Vector2.zero;
            }
        }

        private void FormatInput()
        {
            if (axisOptions == AxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == AxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions != AxisOptions.Both)
                return value switch
                {
                    > 0 => 1,
                    < 0 => -1,
                    _ => 0
                };
            
            var angle = Vector2.Angle(input, Vector2.up);
            return snapAxis switch
            {
                AxisOptions.Horizontal when angle is < 22.5f or > 157.5f => 0,
                AxisOptions.Horizontal => value > 0 ? 1 : -1,
                AxisOptions.Vertical when angle is > 67.5f and < 112.5f => 0,
                AxisOptions.Vertical => value > 0 ? 1 : -1,
                _ => value
            };

        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam,
                    out var localPoint)) return Vector2.zero;

            Vector2 sizeDelta;
            var pivotOffset = baseRect.pivot * (sizeDelta = baseRect.sizeDelta);
            return localPoint - background.anchorMax * sizeDelta + pivotOffset;

        }
    }

    public enum AxisOptions
    {
        Both,
        Horizontal,
        Vertical
    }
}