using System;

namespace Cutscene
{
    public interface ICutsceneTrigger
    {
        public event Action Triggered;
    }
}