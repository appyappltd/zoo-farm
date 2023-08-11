using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Progress;
using Tools.Constants;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class ForceEat : StatChange
    {
        private readonly StatIndicator _barIndicator;
        private readonly float _changingSpeed;
        
        private ProgressBarOperator _bowlBarOperator;

        public ForceEat(AnimalAnimator animator, StatIndicator barIndicator, float changingSpeed)
            : base(animator, barIndicator, changingSpeed)
        {
            _barIndicator = barIndicator;
            _changingSpeed = changingSpeed;
        }

        protected override void PlayAnimation(AnimalAnimator animator) =>
            animator.SetEat();

        protected override void OnEnter()
        {
            _barIndicator.ProgressBar.Reset();
            base.OnEnter();
        }

        protected override void OnExit()
        {
            _barIndicator.ProgressBar.SetMaxNonFull();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            _bowlBarOperator.Update(Time.deltaTime);
        }

        public void SetBowl(Bowl bowl)
        {
            _bowlBarOperator = new ProgressBarOperator(bowl.ProgressBarView, _changingSpeed * Arithmetic.ToHalf, true);
        }
    }
}