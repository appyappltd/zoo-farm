﻿using Joystick_Pack.Scripts.Joysticks;
using UnityEditor;
using UnityEngine;

namespace Joystick_Pack.Editor
{
    [CustomEditor(typeof(VariableJoystick))]
    public class VariableJoystickEditor : JoystickEditor
    {
        private SerializedProperty moveThreshold;
        private SerializedProperty joystickType;

        protected override void OnEnable()
        {
            base.OnEnable();
            moveThreshold = serializedObject.FindProperty("moveThreshold");
            joystickType = serializedObject.FindProperty("joystickType");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (background != null)
            {
                RectTransform backgroundRect = (RectTransform)background.objectReferenceValue;
                backgroundRect.pivot = Center;
            }
        }

        protected override void DrawValues()
        {
            base.DrawValues();
            EditorGUILayout.PropertyField(moveThreshold, new GUIContent("Move Threshold", "The distance away from the center input has to be before the joystick begins to move."));
            EditorGUILayout.PropertyField(joystickType, new GUIContent("Joystick Type", "The type of joystick the variable joystick is current using."));
        }
    }
}