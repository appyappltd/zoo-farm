using System;

namespace Logic.Interactions
{
    public interface IInteractionZone
    { 
        event Action Entered;
        event Action Canceled;
        event Action Rejected;
        event Action Interacted;
        float InteractionDelay { get; }
    }
}