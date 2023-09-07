using NTC.Global.Cache;
using UnityEngine;

namespace Tutorial
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

    public interface IHidable
    {
        void Hide();
    }

    public interface IShowable
    {
        void Show();
    }
}