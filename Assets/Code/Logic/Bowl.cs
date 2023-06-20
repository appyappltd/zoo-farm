using System.Diagnostics;
using NaughtyAttributes;
using Progress;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Logic
{
    public class Bowl : MonoBehaviour, IProgressBarHolder
    {
        [SerializeField] private int _maxFoodCount;

        private ProgressBar _food;

        public IProgressBar ProgressBarView => _food;

        private void Awake()
        {
            Construct(_maxFoodCount);
            Debug.LogWarning("Remove constructor from awake method");
        }

        public void Construct(int maxFoodLevel) =>
            _food = new ProgressBar(maxFoodLevel, 0f);
        
        public void Replenish(int amount)
        {
            _food.Replenish(amount);
            Debug.Log("Replenish" + _food.Current.Value);
        }

        public void Spend(int amount)
        {
            _food.Spend(amount);
            Debug.Log("Spend" + _food.Current.Value);
        }

        [Button("Replenish", EButtonEnableMode.Playmode)] [Conditional("UNITY_EDITOR")]
        public void Replenish1() =>
            Replenish(1);

        [Button("Spend", EButtonEnableMode.Playmode)] [Conditional("UNITY_EDITOR")]
        public void Spend1() =>
            Spend(1);
    }
}