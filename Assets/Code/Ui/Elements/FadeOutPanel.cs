using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Ui.Elements
{
    public class FadeOutPanel : MonoCache, IShowable, IHidable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        private TowardMover<float> _towardMover;

        private void Awake()
        {
            _towardMover = new TowardMover<float>();
        }

        public void Show()
        {
            
        }

        public void Hide()
        {
            
        }
        
        protected override void Run()
        {
            
        }
    }
}