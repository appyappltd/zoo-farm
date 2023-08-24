using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class GoalAnimalPanelProvider : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _animalIcon;
        [SerializeField] private TextSetter _countText;

        public SpriteRenderer AnimalIcon => _animalIcon;
        public TextSetter CountText => _countText;
    }
}