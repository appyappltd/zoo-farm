using System;
using Ui.Factory;
using UnityEngine;

namespace Ui.Services
{
    public class WindowService : IWindowService
    {
        private readonly IUIFactory _uiFactory;

        public WindowService(IUIFactory uiFactory)
        {
            _uiFactory = uiFactory;
        }

        public GameObject Open(WindowId windowId)
        {
            return windowId switch
            {
                WindowId.AnimalRelease => _uiFactory.CreateReleaseAnimalWindow(),
                WindowId.BuildHouse => _uiFactory.CreateBuildHouseWindow(),
                WindowId.Breeding => _uiFactory.CreateBreedingWindow(),
                _ => throw new ArgumentOutOfRangeException(nameof(windowId), windowId, null)
            };
        }
    }
}