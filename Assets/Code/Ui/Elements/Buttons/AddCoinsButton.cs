using Logic.Wallet;
using UnityEngine;

namespace Ui.Elements.Buttons
{
    public class AddCoinsButton : ButtonObserver
    {
        [SerializeField] private int _amountCoinsToAdd;
        [SerializeField] private TextSetter _buttonTestSetter;

        private IWallet _wallet;
        
        public void Construct(IWallet wallet)
        {
            _wallet = wallet;
            UpdateButtonText();
            Subscribe();
        }

        private void OnValidate()
        {
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            _buttonTestSetter.SetText($"Add {_amountCoinsToAdd} coins");
        }

        protected override void Call()
        {
            _wallet.TryAdd(_amountCoinsToAdd);
        }

        private void OnDestroy()
        {
            Cleanup();
        }
    }
}