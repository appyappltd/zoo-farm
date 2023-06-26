using Services;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Ui.Factory
{
  public interface IUIFactory : IService
  {
    void CreateReleaseAnimalWindow();
  }
}