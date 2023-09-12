using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Emotions;
using StateMachineBase.States;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class WaitForFood : Idle
    {
        private readonly PersonalEmotionService _emotionService;

        public WaitForFood(AnimalAnimator animator, PersonalEmotionService emotionService) : base(animator)
        {
            _emotionService = emotionService;
        }

        protected override void OnEnter()
        {
            _emotionService.Suppress(EmotionId.Hunger);
            base.OnEnter();
        }
    }
}