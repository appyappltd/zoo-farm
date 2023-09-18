using UnityEngine;

namespace Logic.Breeding
{
    public struct BreedingPositions
    {
        public readonly Transform First;
        public readonly Transform Second;
        public readonly Transform Child;
        public readonly Transform Center;

        public BreedingPositions(Transform first, Transform second, Transform child, Transform center)
        {
            Center = center;
            First = first;
            Second = second;
            Child = child;
        }
    }
}