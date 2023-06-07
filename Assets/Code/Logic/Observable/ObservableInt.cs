namespace Logic.Observable
{
    public class ObservableInt : ObservableVar<int>
    {
        public ObservableInt(int value) : base(value) { }

        public static implicit operator ObservableInt(int n) => new ObservableInt(n);

        public static implicit operator int(ObservableInt b) => b.Value;
    }
}