using System.Collections.Generic;
using UnityEngine;

namespace Tutorial.TextTutorial
 {
     [CreateAssetMenu(menuName = "Tutorial/Tutorial Text Sequence", fileName = "NewTutorialTextSequence", order = 0)]
     public class TextSequence : ScriptableObject
     {
         private const string End = "TheEnd";
         
         [SerializeField] [TextArea(0, 5)] private List<string> _texts = new List<string>();
 
         public string Next(ref int pointer)
         {
             if (pointer >= _texts.Count)
                 return End;
             
             pointer++;
             return _texts[pointer];
         }
     }
 }