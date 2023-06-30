using System;

namespace Tutorial
{
    public interface ITutorialTrigger
    {
        public event Action Triggered;
    }
}