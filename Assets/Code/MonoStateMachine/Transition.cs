using NTC.Global.Cache;

namespace MonoStateMachine
{
    public abstract class Transition : MonoCache, ITransition
    {
        protected MonoStateMachine StateMachine;

        public void Init(MonoStateMachine stateMachine) =>
            StateMachine = stateMachine;

        public void Enable()
        {
            enabled = true;
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            enabled = false;
            gameObject.SetActive(false);
        }
    }
}