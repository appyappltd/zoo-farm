using NaughtyAttributes;
using UnityEngine;

namespace Ui.Elements
{
    public class BackgroundScaler : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private Vector2 _elementSize;
        [SerializeField] private Vector4 _margin;

        private int _amountElements;
        
        [Button]
        public void AddElement()
        {
            _amountElements++;
            UpdateScale();
        }

        [Button]
        public void RemoveElement()
        {
            _amountElements--;
            UpdateScale();
        }

        private void UpdateScale()
        {
            float horizontalSize = _elementSize.x * _amountElements + _margin.x + _margin.z;
            float verticalSize = _elementSize.y + _margin.y + _margin.w;
            _background.size = new Vector2(horizontalSize, verticalSize);
        }
    }   
}