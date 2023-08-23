using UnityEngine;

namespace Services.Animals
{
    public struct AnimalCountData
    {
        public int Total { get; private set; }
        public int ReleaseReady { get; private set; }

        public AnimalCountData(int total = 0, int releaseReady = 0)
        {
            Total = total;
            ReleaseReady = releaseReady;
        }

        public AnimalCountData AddTotal()
        {
            Total++;
            return this;
        }

        public AnimalCountData RemoveTotal()
        {
            Total = Mathf.Max(0, --Total);
            return this;
        }

        public AnimalCountData AddReleaseReady()
        {
            ReleaseReady = Mathf.Min(++ReleaseReady, Total);
            return this;
        }

        public AnimalCountData RemoveReleaseReady()
        {
            ReleaseReady = Mathf.Max(0, --ReleaseReady);
            return this;
        }

        public override string ToString() =>
            $"Total: {Total}, Ready: {ReleaseReady}";
    }
}