using Services.SaveLoad;

namespace Infrastructure.States
{
    public class FinishState : IState
    {
        private readonly ISaveLoadService _saveLoadService;

        public FinishState() { }

        public void Enter() { }

        public void Exit() { }
    }
}