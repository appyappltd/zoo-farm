namespace MonoStateMachine
{
    public interface ITransition
    {
        void Init(MonoStateMachine stateMachine);
        void Enable();
        void Disable();
    }
}