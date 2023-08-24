using System;
using Logic.Player;

namespace Logic.Interactions
{
    public interface IInteractionZone
    { 
        event Action Entered;
        event Action Canceled;
        event Action Rejected;
        event Action Interacted;
        float InteractionDelay { get; }
        bool IsLooped { get; }
    }
}