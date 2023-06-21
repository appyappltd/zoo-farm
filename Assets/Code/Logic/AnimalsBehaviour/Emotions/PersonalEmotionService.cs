using System.Collections.Generic;
using Services.StaticData;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Emotions
{
    public class PersonalEmotionService : IPersonalEmotionService
    {
        private readonly List<IEmotive> _emotivs = new List<IEmotive>();
        private readonly Stack<Emotion> _emotionsStack = new Stack<Emotion>();
        private readonly List<EmotionId> _suppressEmotionsOrder = new List<EmotionId>();
        private readonly Dictionary<EmotionId, Emotion> _allEmotions = new Dictionary<EmotionId, Emotion>();

        private readonly IStaticDataService _staticDataService;
        private readonly EmotionBubble _emotionBubble;

        public PersonalEmotionService(IStaticDataService staticDataService, EmotionBubble emotionBubble)
        {
            _staticDataService = staticDataService;
            _emotionBubble = emotionBubble;
            _emotionsStack.Push(new Emotion(EmotionId.None, null));
        }

        private EmotionId ActiveEmotionId => _emotionsStack.Peek().Name;
        
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

        private void OnSuppressEmotion(EmotionId emotionId)
        {
            if (ActiveEmotionId == emotionId)
            {
                SuppressEmotions();
            }
            else
            {
                _suppressEmotionsOrder.Add(emotionId);
            }
        }

        private void OnShowEmotion(EmotionId emotionIdType)
        {
            Emotion emotion = _staticDataService.EmotionById(emotionIdType);
            _emotionsStack.Push(emotion);
            _emotionBubble.UpdateEmotion(emotion);
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
            _emotionBubble.UpdateEmotion(_emotionsStack.Pop());
        }
    }
}