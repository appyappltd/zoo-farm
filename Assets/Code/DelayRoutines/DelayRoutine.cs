using System;
using System.Collections.Generic;
using NTC.Global.Cache;

namespace DelayRoutines
{
    public class DelayRoutine
    {
        private readonly GlobalUpdate _globalUpdate;
        private readonly List<IRoutine> _routines = new List<IRoutine>();

        private int _currentRoutineIndex = -1;

        private IRoutine FirstRoutine => _routines[0];
        private IRoutine LastRoutine => _routines[_currentRoutineIndex];
        private IRoutine ActiveRoutine => _routines.Find(routine => routine.IsActive);

        public DelayRoutine() =>
            _globalUpdate = GlobalUpdate.Instance;

        //TODO: Добавить enum для выбора
        public void Play(bool atFirst = true) =>
            _routines[atFirst ? 0 : _currentRoutineIndex].Play();

        public void Stop() =>
            ActiveRoutine.Stop();

        #region Wait

        public DelayRoutine WaitForSeconds(float seconds)
        {
            AddToSequence(new TimeAwaiter(seconds, _globalUpdate));
            return this;
        }

        public DelayRoutine Wait(TimeAwaiter timeAwaiter)
        {
            AddToSequence(timeAwaiter);
            return this;
        }

        public DelayRoutine WaitUntil(Func<bool> waitFor)
        {
            AddToSequence(new UntilAwaiter(waitFor, _globalUpdate));
            return this;
        }

        public DelayRoutine WaitWhile(Func<bool> waitFor)
        {
            AddToSequence(new WhileAwaiter(waitFor, _globalUpdate));
            return this;
        }

        #endregion

        #region Then

        public DelayRoutine Then(Action action)
        {
            AddToSequence(new Executor(action));
            return this;
        }

        public DelayRoutine Then(Executor executor)
        {
            AddToSequence(executor);
            return this;
        }

        #endregion

        #region LoopFor

        public DelayRoutine LoopFor(int times)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);
            return this;
        }

        public DelayRoutine LoopFor(LoopFor loopFor)
        {
            loopFor.AddLoopStart(FirstRoutine);
            AddToSequence(loopFor);
            return this;
        }

        public DelayRoutine LoopFor(int times, IRoutine from)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(from);
            AddToSequence(routine);
            return this;
        }

        public DelayRoutine LoopFor(int times, int fromIndex)
        {
            LoopFor routine = new LoopFor(times);
            routine.AddLoopStart(_routines[fromIndex]);
            AddToSequence(routine);
            return this;
        }

        public DelayRoutine LoopFor(LoopFor loopFor, IRoutine from)
        {
            loopFor.AddLoopStart(from);
            AddToSequence(loopFor);
            return this;
        }

        #endregion

        #region LoopWhile

        public DelayRoutine LoopWhile(Func<bool> repeatCondition)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(FirstRoutine);
            AddToSequence(routine);
            return this;
        }

        public DelayRoutine LoopWhile(Func<bool> repeatCondition, IRoutine from)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(from);
            AddToSequence(routine);
            return this;
        }

        public DelayRoutine LoopWhile(Func<bool> repeatCondition, int fromIndex)
        {
            LoopWhile routine = new LoopWhile(repeatCondition);
            routine.AddLoopStart(_routines[fromIndex]);
            AddToSequence(routine);
            return this;
        }

        public DelayRoutine LoopWhile(LoopWhile loopWhile)
        {
            loopWhile.AddLoopStart(FirstRoutine);
            AddToSequence(loopWhile);
            return this;
        }

        public DelayRoutine LoopWhile(LoopWhile loopWhile, int fromIndex)
        {
            loopWhile.AddLoopStart(_routines[fromIndex]);
            AddToSequence(loopWhile);
            return this;
        }

        #endregion

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