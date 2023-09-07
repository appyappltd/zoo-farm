using System;
using UnityEngine;

namespace Tools
{
    public class TowardMover<TValue>
    {
        private const float FinalValue = 1;
        
        private readonly Func<TValue, TValue, float, TValue> _lerpFunc;
        
        private readonly TValue _from;
        private readonly TValue _to;
        
        private TValue _begin;
        private TValue _end;
        
        private float _delta;
        private bool _isForward = true;
        private bool _isActive;

        private bool IsComplete => _delta > FinalValue;

        public TowardMover(TValue from, TValue to, Func<TValue, TValue, float, TValue> lerp)
        {
            _to = to;
            _from = from;
            _lerpFunc = lerp;
            Reset();
        }

        public bool TryUpdate(float deltaTime, out TValue lerpValue)
        {
            _isActive = true;
            _delta = Mathf.MoveTowards(_delta, FinalValue, deltaTime);
            lerpValue = _lerpFunc.Invoke(_begin, _end, _delta);

            bool _isComplete = IsComplete;

            if (_isComplete)
            {
                _isActive = false;
            }

            return !_isComplete;
        }

        public void Reset()
        {
            _begin = _from;
            _end = _to;
            _delta = 0;
        }

        public void Switch()
        {
            (_begin, _end) = (_end, _begin);
            _delta = FinalValue - _delta;
            _isForward = !_isForward;
        }

        public void Forward()
        {
            if (_isForward == false)
            {
                Switch();
            }
        }

        public void Reverse()
        {
            if (_isForward)
            {
                Switch();
            }
        }
    }
}