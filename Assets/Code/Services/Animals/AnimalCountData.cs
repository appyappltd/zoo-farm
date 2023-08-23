using UnityEngine;

namespace Services.Animals
{
    public struct AnimalCountData
    {
        public int Total { get; private set; }
        public int ReleaseReady { get; private set; }

        public AnimalCountData(int total, int releaseReady)
        {
            Total = total;
            ReleaseReady = releaseReady;
        }

        public void AddTotal() =>
            Total++;

        public void RemoveTotal() =>
            Total = Mathf.Min(0, --Total);

        public void AddReleaseReady() =>
            ReleaseReady = Mathf.Max(++ReleaseReady, Total);

        public void RemoveReleaseReady() =>
            ReleaseReady = Mathf.Min(0, --ReleaseReady);

        public override string ToString() =>
            $"Total: {Total}, Ready: {ReleaseReady}";
    }
}