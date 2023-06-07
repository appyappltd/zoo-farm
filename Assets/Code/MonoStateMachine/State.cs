using System.Collections.Generic;
using System.Linq;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace MonoStateMachine
{
    public abstract class State : MonoCache, IMonoState
    {
        [SerializeField] [RequireInterface(typeof(ITransition))] private List<MonoBehaviour> _toTransitions;

        private readonly List<ITransition> _transitions = new List<ITransition>();

        private void Awake()
        {
            foreach (ITransition transition in _toTransitions.Cast<ITransition>())
                _transitions.Add(transition);

            OnAwake();
        }

        public void EnterBehavior()
        {
            enabled = true;
            EnableTransitions();
        }

        public void ExitBehavior()
        {
            enabled = false;
            DisableTransitions();
        }

        protected virtual void OnAwake() { }

        private void EnableTransitions()
        {
            foreach (ITransition transition in _transitions)
            {
                transition.Enable();
            }
        }

        private void DisableTransitions()
        {
            foreach (ITransition transition in _transitions)
            {
                transition.Disable();
            }
        }
    }
}