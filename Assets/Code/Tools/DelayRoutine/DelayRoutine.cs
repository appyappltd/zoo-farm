using System;
using System.Collections.Generic;
using NTC.Global.Cache;
using NTC.Global.Cache.Interfaces;
using UnityEngine;

namespace Tools.DelayRoutine
{
    public class DelayRoutine
    {
        private readonly GlobalUpdate _globalUpdate;
        private readonly List<IRoutine> _routines = new List<IRoutine>();

        private int _currentRoutineIndex = -1;

        private IRoutine FirstRoutine => _routines[0];
        private IRoutine LastRoutine => _routines[_currentRoutineIndex];

        public DelayRoutine()
        {
            _globalUpdate = GlobalUpdate.Instance;
        }

        public void Play() =>
            _routines[0].Play();

        public DelayRoutine Wait(float forSeconds)
        {
            AddToSequence(new Awaiter(forSeconds, _globalUpdate));
            return this;
        }

        public DelayRoutine Then(Action action)
        {
            AddToSequence(new Executor(action));
            return this;
        }

        public DelayRoutine LoopFor(int times)
        {
            IRoutine routine = new Loop(times);
            routine.AddNext(FirstRoutine);
            AddToSequence(routine);
            return this;
        }

        private void AddToSequence(IRoutine routine)
        {
            if (_currentRoutineIndex >= 0)
            {
                LastRoutine.AddNext(routine);
            }

            _routines.Add(routine);
            _currentRoutineIndex++;
        }
    }
}