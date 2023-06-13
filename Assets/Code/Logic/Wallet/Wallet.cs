using Logic.Observable;
using UnityEngine;

namespace Logic.Wallet
{
    public class Wallet : IWallet
    {
        private const string AddValidateException = "You cannot add negative or zero amount of coins";
        private const string SpendValidateException = "You cannot spend negative or zero amount of coins";
        
        private ObservableInt _account = new ObservableInt();

        public IObservable<int> Account => _account;

        public bool TryAdd(int amount)
        {
            if (Validate(amount, AddValidateException) == false)
                return false;

            _account += amount;
            return true;
        }

        public bool TrySpend(int amount)
        {
            if (Validate(amount, SpendValidateException) == false)
                return false;

            _account -= amount;
            return true;
        }

        private bool Validate(int amount, string exceptionString)
        {
            if (amount > 0)
                return true;

            Debug.LogError(exceptionString);
            return false;

        }
    }
}