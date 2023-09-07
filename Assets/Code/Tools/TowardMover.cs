using System;
using UnityEngine;

namespace Tools
{
    public class TowardMover<TValue>
    {
        private const float FinalValue = 1;
        
        private TValue _from;
        private TValue _to;
        private Func<TValue, TValue, float, TValue> _lerpFunc;
        private float _delta;

        private bool IsComplete => _delta >= FinalValue;

        public void Init(TValue from, TValue to, Func<TValue, TValue, float, TValue> lerpFunc)
        {
            _lerpFunc = lerpFunc;
            _to = to;
            _from = from;
        }
        
        public bool TryUpdate(float deltaTime, out TValue lerpValue)
        {
            _delta = Mathf.MoveTowards(_delta, FinalValue, deltaTime);
            lerpValue = _lerpFunc.Invoke(_from, _to, _delta);
            return IsComplete;
        }

        public void Switch()
        {
            (_from, _to) = (_to, _from);
            _delta = FinalValue - _delta;
        }
    }
}