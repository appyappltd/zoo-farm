using System.Collections.Generic;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.Events;

namespace Cutscene
{
    public class CutsceneDirector : MonoBehaviour
    {
        [SerializeReference] private List<ICutsceneModule> _cutsceneModules = new List<ICutsceneModule>();
        [SerializeReference] private List<UnityEvent> _cutsceneData = new List<UnityEvent>(1);
        [SerializeField] private UnityEvent _unityAction;

        private void Awake()
        {
            _cutsceneData.Add(_unityAction);
            
            _cutsceneModules.Add(new CutsceneAction(() => Debug.Log("Begin Catscene")));
            _cutsceneModules.Add(new CutsceneTimeAwaiter(3f, GlobalUpdate.Instance));
            _cutsceneModules.Add(new CutsceneAction(_unityAction.Invoke));

            for (int i = 0; i < _cutsceneModules.Count - 1; i++)
            {
                _cutsceneModules[i].AttachNext(_cutsceneModules[i + 1]);
            }
        }

        [Button("Play")]
        private void Play()
        {
            _cutsceneModules[0].Play();
        }
    }
}