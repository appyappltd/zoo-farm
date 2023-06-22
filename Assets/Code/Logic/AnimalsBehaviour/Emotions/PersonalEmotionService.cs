using System.Collections.Generic;
using Services.StaticData;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Emotions
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

        private void OnShowEmotion(EmotionId emotionId)
        {
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
            _emotionProvider.ChangeEmotion(_emotionsStack.Pop());
        }
    }
}