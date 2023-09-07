using System;
using UnityEngine;

namespace Tutorial.StaticTriggers
{
    [CreateAssetMenu(menuName = "Tutorial/TutorialTriggerStatic", fileName = "NewTutorialTriggerStatic", order = 0)]
    public class TutorialTriggerScriptableObject : ScriptableObject, ITutorialTriggerPayload
    {
        public event Action Triggered = () => { };
        public event Action<GameObject> TriggeredPayload = o => { };

        public void Trigger()
        {
            Triggered.Invoke();
        }
        
        public void Trigger(GameObject sender)
        {
            Triggered.Invoke();
            TriggeredPayload.Invoke(sender);
        }
        
    }
}