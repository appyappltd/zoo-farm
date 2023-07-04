using UnityEngine;

namespace Observer
{
    [RequireComponent(typeof(TriggerObserverExit))]
    public abstract class ObserverTargetExit<TTarget, TObserver> : ObserverTarget<TTarget, TObserver> where TObserver : ITriggerObserverExit
    {
        protected override void OnEnabled()
        {
            base.OnEnabled();
            Observer.Exited += OnTriggerObserverExitExited;
        }

        protected override void OnDisabled()
        {
            base.OnDisabled();
            Observer.Exited -= OnTriggerObserverExitExited;
        }

        protected abstract void OnTargetExited(TTarget target);

        protected override void GetObserver() =>
            Observer = GetComponent<TObserver>();

        private void OnTriggerObserverExitExited(Collider other)
        {
            if (other.TryGetComponent(out TTarget target))
            {
                OnTargetExited(target);
            }
        }
    }
}
