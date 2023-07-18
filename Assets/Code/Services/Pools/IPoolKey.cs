namespace Services.Pools
{
    public readonly struct PoolKey
    {
        public readonly int Key;
        public PoolKey(object key)
        {
            Key = key.GetHashCode();
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}