using System.Collections.Generic;
using Logic.Storages;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public interface IInteractionValidator
    {
        public bool IsValid(IInventory inventory = default);
    }
    
    public interface IInteractionValidatorSO : IInteractionValidator
    {
        public IReadOnlyList<MonoBehaviour> References { get; }
    }
}