namespace Services.Animals
{
    public struct AnimalCountData
    {
        public int Total { get; }
        public int ReleaseReady { get; }

        public AnimalCountData(int total, int releaseReady)
        {
            Total = total;
            ReleaseReady = releaseReady;
        }

        public override string ToString()
        {
            return $"Total: {Total}, Ready: {ReleaseReady}";
        }
    }
}