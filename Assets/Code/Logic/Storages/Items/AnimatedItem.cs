using System.Collections.Generic;
using Logic.Translators;
using UnityEngine;
using UnityEngine.Pool;

namespace Logic.Storages.Items
{
    public class AnimatedItem : HandItem, ITranslatableAnimated
    {
        [SerializeField] private CustomScaleTranslatable _scaleTranslatable;
        [SerializeField] private CustomPositionTranslatable _positionTranslatable;
        [SerializeField] private CustomRotationTranslator _rotationTranslator;

        public ITranslatableParametric<Vector3> ScaleTranslatable => _scaleTranslatable;
        public ITranslatableParametric<Vector3> PositionTranslatable => _positionTranslatable;
        public ITranslatableParametric<Vector3> RotationTranslator => _rotationTranslator;

        public IEnumerable<ITranslatableParametric<Vector3>> GetAllTranslatables()
        {
            using (ListPool<ITranslatableParametric<Vector3>>.Get(out var tempList))
            {
                TryAdd(tempList, _scaleTranslatable);
                TryAdd(tempList, _positionTranslatable);
                TryAdd(tempList, _rotationTranslator);

                return tempList.ToArray();
            }
        }

        private void TryAdd(List<ITranslatableParametric<Vector3>> tempList, ITranslatableParametric<Vector3> translatable)
        {
            if (translatable is not null)
            {
                tempList.Add(translatable);
            }
        }
    }
}