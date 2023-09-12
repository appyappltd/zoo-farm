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

            _satietyBar.Empty += ShowEmotion;
            _satietyBar.Full += SuppressEmotion;
            
            _emotionService.Show(EmotionId.Hunger);
        }

        private void SuppressEmotion() =>
            _emotionService.Suppress(EmotionId.Hunger);

        private void ShowEmotion() =>
            _emotionService.Show(EmotionId.Hunger);

        public void Dispose()
        {
            _satietyBar.Empty -= ShowEmotion;
            _satietyBar.Full -= SuppressEmotion;
        }
    }
}