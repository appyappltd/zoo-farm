namespace Logic.Storages
{
    public readonly struct ItemFilter
    {
        private const int FullMack = 1111111111;
        
        private readonly int _mailFilter;
        private readonly int _subFilter;

        public ItemFilter(int mailFilter, int subFilter)
        {
            _mailFilter = mailFilter;
            _subFilter = subFilter;
        }
        
        public ItemFilter(int mailFilter)
        {
            _mailFilter = mailFilter;
            _subFilter = FullMack;
        }
        
        public bool Contains(int mainMask, int subMask)
        {
            return (_mailFilter & mainMask) != 0 && (_subFilter & subMask) != 0;
        }
        
        public bool Contains(int mainMask)
        {
            return (_mailFilter & mainMask) != 0;
        }
    }
}