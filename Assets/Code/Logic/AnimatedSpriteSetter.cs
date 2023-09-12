using AYellowpaper;
using Tools;
using UnityEngine;

namespace Logic
{
    public class AnimatedSpriteSetter : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private InterfaceReference<IShowHide, MonoBehaviour> _showHidable;

        public void ChangeSprite(Sprite newSprite)
        {
            _showHidable.Value.Hide(() => SetSprite(newSprite));
            _showHidable.Value.Show();
        }

        private void SetSprite(Sprite sprite) =>
            _spriteRenderer.sprite = sprite;
    }
}