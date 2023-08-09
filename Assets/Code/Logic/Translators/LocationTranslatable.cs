using System;
using UnityEngine;

namespace Logic.Translators
{
    public class LocationTranslatable : Translatable<Location>
    {
        [SerializeField] private AnimationCurve _timeCurve;
        
        protected override void ApplyModifiers() =>
            AddDeltaModifier(_timeCurve.Evaluate);

        protected override void ApplyTranslation(Location location)
        {
            Transform selfTransform = transform;
            selfTransform.localPosition = location.Position;
            selfTransform.localRotation = location.Rotation;
        }

        protected override void SetValueLerp(ref Func<Location, Location, float, Location> valueLerp) =>
            valueLerp = Lerp;

        private Location Lerp(Location a, Location b, float t) => 
            new Location(
                Vector3.SlerpUnclamped(a.Position, b.Position, t),
                Quaternion.SlerpUnclamped(a.Rotation, b.Rotation, t));
    }
}
