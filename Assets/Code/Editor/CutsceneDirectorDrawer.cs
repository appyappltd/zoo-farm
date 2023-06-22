using System.Collections.Generic;
using Cutscene;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    [CustomEditor(typeof(CutsceneDirector))]
    public class CutsceneDirectorDrawer : UnityEditor.Editor
    {
        private const string CutsceneModules = "_cutsceneModules";

        private List<ICutsceneData> _cutsceneData = new List<ICutsceneData>();

        private SerializedProperty _modules;
        private CutsceneDirector _target;

        private void OnEnable()
        {
            // _target = (CutsceneDirector) target;
            _modules = serializedObject.FindProperty(CutsceneModules);
            // Debug.Log(_modules);
        }

        public override void OnInspectorGUI()
        {
            for (int i = 0; i < _modules.arraySize; i++)
            {
                object module = _modules.GetArrayElementAtIndex(i).managedReferenceValue;

                if (module is CutsceneActionData cutsceneAction)
                {
                    Debug.Log("action");
                    DrawActionModule(cutsceneAction);
                }

                if (module is CutsceneTimeAwaiterData cutsceneTimeAwaiter)
                {
                    Debug.Log("awaiter");
                    DrawAwaitModule(cutsceneTimeAwaiter);
                }
            }
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