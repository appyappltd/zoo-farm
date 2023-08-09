using Services;
using UnityEngine;

namespace Ui.Factory
{
    public interface IUIFactory : IService
    {
        void CreateUIRoot();
        GameObject CreateReleaseAnimalWindow();
        GameObject CreateBuildHouseWindow();
        GameObject CreateBreedingWindow();
        ChoseAnimalPanel CreateChoseAnimalPanel(Transform parent);
        ReleaseAnimalPanel CreateReleaseAnimalPanel(Transform panelsParent);
    }
}