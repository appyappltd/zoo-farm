using Observables;
using UnityEngine;

namespace Logic.Payment
{
    public class Wallet : IWallet
    {
        private const string AddValidateException = "You cannot add negative or zero amount of coins";
        private const string SpendValidateException = "You cannot spend negative or zero amount of coins";

        private readonly Observable<int> _account = new Observable<int>();

        public Observable<int> Account => _account;

        public bool TryAdd(int amount)
        {
            if (Validate(amount, AddValidateException) == false)
                return false;

            _account.Value += amount;
            return true;
        }

        public bool TrySpend(int amount)
        {
            if (Validate(amount, SpendValidateException) == false)
                return false;

            if (_account.Value - amount < 0)
                return false;

            _account.Value -= amount;
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