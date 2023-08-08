using Logic.AnimatorStateMachine;
using Logic.Storages;
using StateMachineBase.States;

namespace Code.Logic.NPC.Keepers.KeepersStateMachine.States
{
    public class ReceiveItems : Idle
    {
        private readonly IInventory _inventory;

        public ReceiveItems(IPrimeAnimator animator, IInventory inventory) : base(animator)
        {
            _inventory = inventory;
        }

        protected override void OnEnter()
        {
            _inventory.Activate();
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _inventory.Deactivate();
            base.OnExit();
        }
    }
}