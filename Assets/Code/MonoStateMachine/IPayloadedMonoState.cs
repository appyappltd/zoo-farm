namespace MonoStateMachine
{
    public interface IPayloadedMonoState<TPayLoad> : IMonoState
    {
        void EnterBehavior(TPayLoad payload);
    }
}