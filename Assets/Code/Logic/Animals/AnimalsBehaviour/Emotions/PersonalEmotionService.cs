using System.Collections.Generic;

namespace Logic.Animals.AnimalsBehaviour.Emotions
{
    public class PersonalEmotionService : IPersonalEmotionService
    {
        private readonly List<IEmotive> _emotivs = new List<IEmotive>();
        private readonly Stack<EmotionId> _emotionsStack = new Stack<EmotionId>();
        private readonly List<EmotionId> _suppressEmotionsOrder = new List<EmotionId>();

        private readonly EmotionProvider _emotionProvider;

        public PersonalEmotionService(EmotionProvider emotionProvider)
        {
            _emotionProvider = emotionProvider;
            _emotionsStack.Push(EmotionId.None);
        }

        private EmotionId ActiveEmotionId => _emotionsStack.Peek();

        public void Register(IEmotive emotive)
        {
            if (_emotivs.Contains(emotive))
                return;

            _emotivs.Add(emotive);
            Subscribe(emotive);
        }

        public void Unregister(IEmotive emotive)
        {
            if (_emotivs.Contains(emotive) == false)
                return;

            _emotivs.Remove(emotive);
            Unsubscribe(emotive);
        }

        public void Suppress(EmotionId emotion) =>
            OnSuppressEmotion(emotion);

        public void Show(EmotionId emotion) =>
            OnShowEmotion(emotion);

        private void Subscribe(IEmotive emotive)
        {
            emotive.ShowEmotion += OnShowEmotion;
            emotive.SuppressEmotion += OnSuppressEmotion;
        }

        private void Unsubscribe(IEmotive emotive)
        {
            emotive.ShowEmotion -= OnShowEmotion;
            emotive.SuppressEmotion -= OnSuppressEmotion;
        }

        private void OnSuppressEmotion(EmotionId emotionId)
        {
            if (ActiveEmotionId == emotionId)
                SuppressEmotions();
            else
                _suppressEmotionsOrder.Add(emotionId);
        }

        private void OnShowEmotion(EmotionId emotionId)
        {
            if (ActiveEmotionId == emotionId)
                return;

            _emotionsStack.Push(emotionId);
            _emotionProvider.ChangeEmotion(emotionId);
        }

        private void SuppressEmotions()
        {
            SuppressActiveEmotion();

            while (_suppressEmotionsOrder.Contains(ActiveEmotionId))
            {
                _suppressEmotionsOrder.Remove(ActiveEmotionId);
                SuppressActiveEmotion();
            }
        }

        private void SuppressActiveEmotion()
        {
            _emotionsStack.Pop();
            _emotionProvider.ChangeEmotion(_emotionsStack.Peek());
        }
    }
}