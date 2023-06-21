using System.Collections.Generic;

namespace Logic.AnimalsBehaviour.Emotions
{
    public class PersonalEmotionService : IPersonalEmotionService
    {
        private readonly List<IEmotive> _emotivs = new List<IEmotive>();
        private readonly Stack<Emotion> _emotionsStack = new Stack<Emotion>();
        private readonly List<Emotions> _suppressEmotionsOrder = new List<Emotions>();

        private readonly EmotionBubble _emotionBubble;

        public PersonalEmotionService(EmotionBubble emotionBubble)
        {
            _emotionBubble = emotionBubble;
            _emotionsStack.Push(new Emotion(Emotions.None, null));
        }

        private Emotions ActiveEmotion => _emotionsStack.Peek().Name;
        
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

        private void OnSuppressEmotion(Emotions emotion)
        {
            if (ActiveEmotion == emotion)
            {
                SuppressEmotions();
            }
            else
            {
                _suppressEmotionsOrder.Add(emotion);
            }
        }

        private void OnShowEmotion(Emotions emotionType)
        {
            Emotion emotion = new Emotion(emotionType, null);
            _emotionsStack.Push(emotion);
            _emotionBubble.UpdateEmotion(emotion);
            
        }

        private void SuppressEmotions()
        {
            SuppressActiveEmotion();

            while (_suppressEmotionsOrder.Contains(ActiveEmotion))
            {
                _suppressEmotionsOrder.Remove(ActiveEmotion);
                SuppressActiveEmotion();
            }
        }

        private void SuppressActiveEmotion()
        {
            _emotionBubble.UpdateEmotion(_emotionsStack.Pop());
        }
    }
}