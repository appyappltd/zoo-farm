using Logic.Wallet;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class BuildIconView : MonoBehaviour
    {
        [SerializeField] private Consumer _consumer;
        [SerializeField] private TextSetter _costText;

        private void Start()
        {
            UpdateText(_consumer.LeftCoinsToPay.Value);
            _consumer.LeftCoinsToPay.Then(UpdateText);
        }

        private void UpdateText(int costLeft) =>
            _costText.SetText(costLeft);
    }
}