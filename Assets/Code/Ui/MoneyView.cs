using System;
using Observables;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class MoneyView : MonoBehaviour
    {
        [SerializeField] private TextSetterAnimated _moneyAmount;

        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();

        private void OnDestroy() =>
            _compositeDisposable.Dispose();

        public void Construct(Observable<int> walletAccount)
        {
            IDisposable disposable = walletAccount.Then(OnMoneyChanged);
            _compositeDisposable.Add(disposable);
            _moneyAmount.SetTextAnimated(walletAccount.Value);
        }

        private void OnMoneyChanged(int value) =>
            _moneyAmount.SetTextAnimated(value);
    }
}