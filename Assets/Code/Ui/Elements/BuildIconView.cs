using Logic.CellBuilding;
using UnityEngine;

namespace Ui.Elements
{
    public class BuildIconView : MonoBehaviour
    {
        [SerializeField] private BuildCell _buildCell;
        [SerializeField] private TextSetter _costText;

        private void Start()
        {
            _costText.SetText(_buildCell.BuildCost);
            transform.forward = Camera.main.transform.forward;
        }
    }
}