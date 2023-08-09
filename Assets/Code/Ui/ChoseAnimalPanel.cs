using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Ui
{
    public class ChoseAnimalPanel : MonoBehaviour
    {
        [SerializeField] private Image _animalIcon;
        [SerializeField] private Button _button;

        private void OnDestroy() =>
            _button.onClick.RemoveAllListeners();

        public void Construct(Sprite icon, UnityAction onClickCallback)
        {
            _animalIcon.sprite = icon;
            _button.onClick.AddListener(onClickCallback);
        }
    }
}