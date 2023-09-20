using System.Text;
using Data.AnimalCounter;
using Logic.Animals;
using Services;
using Services.Animals;
using UnityEngine;
using Debug = Sisus.Debugging.Debug;

namespace Tools
{
    public class AnimalCounterDataInfoDrawer : MonoBehaviour
    {
        GUIContent _content;
        private Rect _rect;
        private IAnimalCounter _counter;
        private GUIStyle _style;

        void Awake()
        {
            var animalService = AllServices.Container.Single<IAnimalsService>();
            _counter = animalService.AnimalCounter;
            _counter.Updated += Resize;
        
            _content = GUIContent.none;
        
            _style = new("Box")
            {
                alignment = TextAnchor.MiddleLeft,
                wordWrap = true,
                stretchWidth = true,
                stretchHeight = true,
                fontSize = 30
            };
        }

        private void OnDestroy()
        {
            _counter.Updated -= Resize;
        }

        void OnGUI()
        {
            GUI.Box(_rect, _content, _style);
        }

        private void Resize(AnimalType _, AnimalCountData __)
        {
            StringBuilder sb = new StringBuilder();
        
            foreach (var (key, value) in _counter.GetAllData()) 
                sb.Append($"Animal {key}: Ready/Total {value.ReleaseReady}/{value.Total}\n");
        
            _content = new GUIContent(sb.ToString());

            var size = _style.CalcSize(_content);
            _rect = new Rect(50, 50, size.x, size.y);
        }
    }
}