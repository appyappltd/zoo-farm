using System;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialTriggerStaticContainer : MonoBehaviour
    {
        [SerializeField] private TutorialTriggerStatic _trigger;
        [SerializeField] private bool _IsTriggerOnEnable;
        [SerializeField] private bool _IsTriggerOnDisable;

        private void OnEnable()
        {
            if (_IsTriggerOnEnable)
                Trigger();
        }

        private void OnDisable()
        {
            if (_IsTriggerOnDisable)
                Trigger();
        }
        
        public void Trigger()
        {
            if (_trigger is null)
                throw new NullReferenceException("Trigger object is not set");

            _trigger.Trigger(gameObject);
        }
    }
}