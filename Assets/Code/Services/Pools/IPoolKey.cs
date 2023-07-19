namespace Services.Pools
{
    public readonly struct PoolKey
    {
        public readonly int Key;
        public PoolKey(object key)
        {
            if (key.GetType().IsClass)
            {
                Key = key.GetHashCode();
            }
            else
            {
                Key = key.ToString().GetHashCode();
            }
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}