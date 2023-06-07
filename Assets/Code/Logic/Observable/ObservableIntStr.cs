using System;

namespace Logic.Observable
{
    public struct ObservableIntStr : IObservable<int>
    {
        private int _value;

        public event Action<int> Changed;

        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                Changed?.Invoke(_value);
            }
        }

        public ObservableIntStr(int value) : this() => _value = value;

        public static implicit operator ObservableIntStr(int n) => new ObservableIntStr(n);

        public static implicit operator int(ObservableIntStr b) => b.Value;
    }
}