using Services;
using UnityEngine;

namespace Ui.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        GameObject CreateReleaseAnimalWindow();
        GameObject CreateBuildHouseWindow();
        BuildHousePanel CreateBuildHousePanel(Transform parent);
        ReleaseAnimalPanel CreateReleaseAnimalPanel(Transform panelsParent);
    }
}