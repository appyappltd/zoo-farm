using System.Diagnostics;
using NaughtyAttributes;
using Progress;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Logic
{
    public class Bowl : MonoBehaviour
    {
        [SerializeField] private int _maxFoodCount;

        private ProgressBar _food;

        public IProgressBarView FoodBar => _food;

        private void Awake()
        {
            Construct(_maxFoodCount);
            Debug.LogWarning("Remove constructor from awake method");
        }

        public void Construct(int maxFoodLevel) =>
            _food = new ProgressBar(maxFoodLevel, 0);
        
        public void Replenish(int amount) =>
            _food.Replenish(amount);
        
        public void Spend(int amount) =>
            _food.Spend(amount);

        [Button("Replenish", EButtonEnableMode.Playmode)] [Conditional("UNITY_EDITOR")]
        public void Replenish1() =>
            Replenish(1);

        [Button("Spend", EButtonEnableMode.Playmode)] [Conditional("UNITY_EDITOR")]
        public void Spend1() =>
            Spend(1);
    }
}