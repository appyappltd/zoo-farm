using Services;

namespace Ui.Factory
{
  public interface IUIFactory : IService
  {
    void CreateReleaseAnimalWindow();
    void CreateUIRoot();
  }
}