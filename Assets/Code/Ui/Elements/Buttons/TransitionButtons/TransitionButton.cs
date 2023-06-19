using Infrastructure.States;
using UnityEngine;
using UnityEngine.UI;

namespace Ui.Elements.Buttons.TransitionButtons
{
    [RequireComponent(typeof(Button))]
    public abstract class TransitionButton : ButtonObserver
    {
        protected GameStateMachine StateMachine;

        public void Construct(GameStateMachine stateMachine) =>
            StateMachine = stateMachine;

        protected override void Call() =>
            StateMachine.Enter<LoadLevelState, string>(TransitionPayload());

        protected abstract string TransitionPayload();
    }
}