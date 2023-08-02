using AYellowpaper.SerializedCollections;
using Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView;
using UnityEngine;
 
 namespace Logic.Animals.AnimalsBehaviour.Emotions
 {
     public class EmotionProvider : MonoBehaviour
     {
         private EmotionView _currentEmotion;
 
         [SerializeField] private SerializedDictionary<EmotionId, EmotionView> _emotions;
 
         private void Awake()
         {
             _currentEmotion = _emotions[EmotionId.Hunger];
         }
 
         public void ChangeEmotion(EmotionId id)
         {
             _currentEmotion.Hide();
             _currentEmotion = _emotions[id];
             _currentEmotion.Show();
         }
     }
 }