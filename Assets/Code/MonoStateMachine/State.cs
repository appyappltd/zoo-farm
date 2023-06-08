using System.Collections.Generic;
using System.Linq;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace MonoStateMachine
{
    public abstract class State : MonoCache, IMonoState
    {
        [SerializeField] [RequireInterface(typeof(ITransition))] private List<MonoBehaviour> _fromTransitions;

        private readonly List<ITransition> _transitions = new List<ITransition>();

        private void Awake()
        {
            foreach (ITransition transition in _fromTransitions.Cast<ITransition>())
                _transitions.Add(transition);

            OnAwake();
        }

        protected abstract void OnEnter();

        protected abstract void OnExit();

        public void EnterBehavior()
        {
            gameObject.SetActive(true);
            EnableTransitions();
            OnEnter();
        }

        public void ExitBehavior()
        {
            gameObject.SetActive(false);
            DisableTransitions();
            OnExit();
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