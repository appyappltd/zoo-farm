using System;
using Ui.Factory;

namespace Ui.Services
{
  public class WindowService : IWindowService
  {
    private readonly IUIFactory _uiFactory;

    public WindowService(IUIFactory uiFactory)
    {
      _uiFactory = uiFactory;
    }

    public void Open(WindowId windowId)
    {
      switch (windowId)
      {
        case WindowId.None:
          break;
        case WindowId.AnimalRelease:
          _uiFactory.CreateReleaseAnimalWindow();
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null);
      }
    }
  }
}