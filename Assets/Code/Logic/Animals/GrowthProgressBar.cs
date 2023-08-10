using Logic.SpriteUtils;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Animals
{
    public class GrowthProgressBar : MonoBehaviour
    {
        [SerializeField] private SpriteFillMask _fillMask;

        private int _stagesCount;
        private int _currentStage;

        public void Construct(int stagesCount)
        {
            _stagesCount = stagesCount;
            _currentStage = 0;
        }

        [Button]
        public void NextStage()
        {
            _currentStage++;
            _fillMask.SetFill(_currentStage / (float) _stagesCount);
        }
    }
}