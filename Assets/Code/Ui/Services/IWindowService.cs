using Services;

namespace Ui.Services
{
  public interface IWindowService : IService
  {
    void Open(WindowId windowId);
  }
}