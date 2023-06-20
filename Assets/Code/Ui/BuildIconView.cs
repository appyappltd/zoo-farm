using Logic.CellBuilding;
using Ui.Elements;
using UnityEngine;

namespace Ui
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