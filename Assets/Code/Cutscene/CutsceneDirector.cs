using System.Collections.Generic;
using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Cutscene
{
    public abstract class CutsceneDirector : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(ICutsceneTrigger))] private MonoBehaviour _beginTrigger;
        
        protected readonly List<ICutsceneModule> CutsceneModules = new List<ICutsceneModule>();

        private void OnDestroy()
        {
            ((ICutsceneTrigger) _beginTrigger).Triggered -= Play;
        }
        
        protected abstract void CollectModules();
        
        [Button("Play")]
        public void Play()
        {
            CollectModules();
            
            for (int i = 0; i < CutsceneModules.Count - 1; i++)
            {
                CutsceneModules[i].AttachNext(CutsceneModules[i + 1]);
            }
            
            ((ICutsceneTrigger) _beginTrigger).Triggered += Play;
            
            CutsceneModules[0].Play();
        }
    }
}