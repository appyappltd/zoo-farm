using Services;
using UnityEngine;

namespace Ui.Services
{
  public interface IWindowService : IService
  {
    GameObject Open(WindowId windowId);
  }
}