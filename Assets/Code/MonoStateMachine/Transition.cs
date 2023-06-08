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
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}