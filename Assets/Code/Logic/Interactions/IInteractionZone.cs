using System;
using Logic.Player;

namespace Logic.Interactions
{
    public interface IInteractionZone<out T> where T : IHuman
    { 
        event Action Entered;
        event Action Canceled;
        event Action Rejected;
        event Action<T> Interacted;
        float InteractionDelay { get; }
    }
}