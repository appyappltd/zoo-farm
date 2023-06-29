using System;
using System.Collections.Generic;
using Tutorial;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Editor
{
    [CustomEditor(typeof(TutorialDirector))]
    public class CutsceneDirectorDrawer : UnityEditor.Editor
    {
        private const string CutsceneModules = "_cutsceneModules";

        private SerializedProperty _modules;
        private TutorialDirector _target;
        private SerializedProperty _events;

        private void OnEnable()
        {
            // _target = (CutsceneDirector) target;
            _modules = serializedObject.FindProperty(CutsceneModules);
            _events = serializedObject.FindProperty("_cutsceneData");
            // Debug.Log(_modules);
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(_events.GetArrayElementAtIndex(0));
            
            // for (int i = 0; i < _modules.arraySize; i++)
            // {
            //     object module = _modules.GetArrayElementAtIndex(i).managedReferenceValue;
            //
            //     if (module is CutsceneActionData cutsceneAction)
            //     {
            //         Debug.Log("action");
            //         EditorGUILayout.PropertyField(_events.GetArrayElementAtIndex(i));
            //         DrawActionModule(cutsceneAction);
            //     }
            //
            //     if (module is CutsceneTimeAwaiterData cutsceneTimeAwaiter)
            //     {
            //         Debug.Log("awaiter");
            //         DrawAwaitModule(cutsceneTimeAwaiter);
            //     }
            // }
            
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAwaitModule(CutsceneTimeAwaiterData module)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.IntField(module.WaitTime);
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawActionModule(CutsceneActionData module)
        {
            // EditorGUILayout.PropertyField(, "Actions", false);
        }
    }
}