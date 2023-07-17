using Services;
using UnityEngine;

namespace Ui.Factory
{
    public interface IUIFactory : IService
    {
        GameObject CreateReleaseAnimalWindow();
        void CreateUIRoot();
        BuildHousePanel CreateBuildHousePanel(Transform parent);
        GameObject CreateBuildHouseWindow();
    }
}