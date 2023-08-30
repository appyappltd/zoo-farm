using AYellowpaper;
using Logic.Payment;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class BuildIconView : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IConsumer, MonoBehaviour> _consumer;
        [SerializeField] private TextSetter _costText;

        private void Start()
        {
            UpdateText(_consumer.Value.LeftCoinsToPay.Value);
            _consumer.Value.LeftCoinsToPay.Then(UpdateText);
        }

        private void UpdateText(int costLeft) =>
            _costText.SetText(costLeft);
    }
}