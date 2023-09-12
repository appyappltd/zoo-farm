using System;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Progress;

namespace Logic.Animals.AnimalsBehaviour
{
    public class SatietyEmotionObserver : IDisposable
    {
        private readonly IProgressBarView _satietyBar;
        private readonly PersonalEmotionService _emotionService;

        public SatietyEmotionObserver(IProgressBarView satietyBar, PersonalEmotionService emotionService)
        {
            _satietyBar = satietyBar;
            _emotionService = emotionService;

            _satietyBar.Empty += OnEmpty;
            _satietyBar.Full += OnFull;
            
            _emotionService.Show(EmotionId.Hunger);
        }

        private void OnFull()
        {
            _emotionService.Suppress(EmotionId.Hunger);
            _emotionService.Show(EmotionId.Happy);
        }

        private void OnEmpty()
        {
            _emotionService.Suppress(EmotionId.Happy);
            _emotionService.Show(EmotionId.Hunger);
        }

        public void Dispose()
        {
            _satietyBar.Empty -= OnEmpty;
            _satietyBar.Full -= OnFull;
        }
    }
}