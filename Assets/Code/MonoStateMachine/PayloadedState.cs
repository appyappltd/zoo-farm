namespace MonoStateMachine
{
    public abstract class PayloadedState<TPayload> : State, IPayloadedMonoState<TPayload>
    {
        public void EnterBehavior(TPayload payload)
        {
            OnPayloadEnter(payload);
            EnterBehavior();
        }

        protected abstract void OnPayloadEnter(TPayload payload);
    }
}