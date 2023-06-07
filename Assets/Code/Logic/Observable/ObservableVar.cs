using System;

namespace Logic.Observable
{
    public class ObservableVar<T> : IObservable<T>
    {
        private T value;

        public event Action<T> Changed;

        public ObservableVar(T value) => this.value = value;

        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                Changed?.Invoke(this.value);
            }
        }
    }
}