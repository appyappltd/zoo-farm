using Logic.Observable;
using UnityEngine;

namespace Logic.Wallet
{
    public class Wallet : IWallet
    {
        private const string AddValidateException = "You cannot add negative amount of coins";
        private const string SpendValidateException = "You cannot spend negative amount of coins";
        
        private ObservableInt account;

        public Observable.IObservable<int> Account => account;

        public bool TryAdd(int amount)
        {
            if (Validate(amount, AddValidateException) == false)
                return false;

            account += amount;
            return true;

        }

        public bool TrySpend(int amount)
        {
            if (Validate(amount, SpendValidateException) == false)
                return false;

            account -= amount;
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