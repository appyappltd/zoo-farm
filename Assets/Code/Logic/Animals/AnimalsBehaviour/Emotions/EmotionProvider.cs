using AYellowpaper.SerializedCollections;
using Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView;
using Logic.Foods;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions
 {
     public class EmotionProvider : MonoBehaviour
     {
         private EmotionView _currentEmotion;
 
         [SerializeField] private SerializedDictionary<EmotionId, EmotionView> _emotions;
         [SerializeField] private FoodShower _foodShower;

         private void Awake() =>
             _currentEmotion = _emotions[EmotionId.None];

         public void Construct(FoodId foodId) =>
             _foodShower.SetMaterial(foodId);

         public void ChangeEmotion(EmotionId id)
         {
             _currentEmotion.Hide();
             _currentEmotion = _emotions[id];
             _currentEmotion.Show();
         }
     }
 }