using System.Collections.Generic;
using UnityEngine;

namespace Logic.Translators
{
    public interface ITranslatableAnimated
    {
        ITranslatableParametric<Vector3> ScaleTranslatable { get; }
        ITranslatableParametric<Vector3> PositionTranslatable { get; }
        ITranslatableParametric<Vector3> RotationTranslator { get; }

        IEnumerable<ITranslatableParametric<Vector3>> GetAllTranslatables();
    }
}