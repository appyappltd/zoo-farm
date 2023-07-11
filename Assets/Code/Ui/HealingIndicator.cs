using UnityEngine;

namespace Ui
{
    public class HealingIndicator : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void Show()
        {
            _spriteRenderer.enabled = true;
        }

        public void Hide()
        {
            _spriteRenderer.enabled = false;
        }
    }
}