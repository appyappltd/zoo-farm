using System.Collections.Generic;
using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Tutorial
{
    public abstract class TutorialDirector : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(ITutorialTrigger))] private MonoBehaviour _beginTrigger;
        
        protected readonly List<ITutorialModule> TutorialModules = new List<ITutorialModule>();

        private void Awake()
        {
            ((ITutorialTrigger) _beginTrigger).Triggered += Play;
        }

        protected abstract void CollectModules();
        
        [Button("Play")]
        public void Play()
        {
            CollectModules();
            
            for (int i = 0; i < TutorialModules.Count - 1; i++)
            {
                TutorialModules[i].AttachNext(TutorialModules[i + 1]);
            }

            ((ITutorialTrigger) _beginTrigger).Triggered -= Play;
            TutorialModules[0].Play();
        }
    }
}