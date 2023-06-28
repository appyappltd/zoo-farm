namespace Logic.AnimatorStateMachine
{
    public interface IPrimeAnimator
    {
        void SetIdle();
        void SetMove();
        void SetSpeed(float speed);
    }
}