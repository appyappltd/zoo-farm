using Infrastructure.AssetManagement;
using Services.Animals;
using Services.PersistentProgress;
using Services.StaticData;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace Ui.Factory
{
    public class UIFactory : IUIFactory
    {
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly IAnimalsService _animalsService;

    private Transform _uiRoot;
    
    public UIFactory(IAssetProvider assets, IStaticDataService staticData,
        IPersistentProgressService progressService, IAnimalsService animalsService)
    {
        _assets = assets;
        _staticData = staticData;
        _progressService = progressService;
        _animalsService = animalsService;
    }

    // public void CreateSettings()
    // {
    //     var window = CreateWindow<SettingsWindow>(WindowId.Settings);
    //     window.Construct(_localizationService, _soundService);
    // }
    //
    // public void CreatePause()
    // {
    //     var window = CreateWindow<PauseWindow>(WindowId.Pause);
    //     window.Construct(_progressService, _pauseService, _stateMachine);
    //     _pauseService.Register(window);
    // }
    //
    // public void CreateResults()
    // {
    //     var window = CreateWindow<ResultsWindow>(WindowId.Results);
    //     window.Construct(_progressService, _stateMachine);
    // }
    //
    // public void CreateLose()
    // {
    //     var window = CreateWindow<LoseWindow>(WindowId.Lose);
    //     window.Construct(_progressService, _scoreService, _stateMachine);
    // }
    //
    // public void CreateLeaderboard()
    // {
    //     var window = CreateWindow<LeaderboardWindow>(WindowId.Leaderboard);
    //     window.Construct(_rankedService, this);
    // }
    //

    public void CreateReleaseAnimalWindow()
    {
        AnimalReleaseWindow window = CreateWindow<AnimalReleaseWindow>(WindowId.AnimalRelease);
        window.Construct(_animalsService);
    }

    public void CreateUIRoot()
    {
        GameObject root = _assets.Instantiate(AssetPath.UIRootPath);
        _uiRoot = root.transform;
        root.GetComponent<Canvas>().worldCamera = Camera.main;
    }

    private TWindow CreateWindow<TWindow>(WindowId windowId) where TWindow : WindowBase
    {
        WindowBase template = _staticData.WindowById(windowId);
        TWindow window = Object.Instantiate(template, _uiRoot) as TWindow;
        return window;
    }

    private GameObject InstantiateRegistered(string path, Transform inside)
    {
        GameObject element = _assets.Instantiate(path, inside);
        Register(element);
        return element;
    }

    private void Register(GameObject element)
    {
    }
    }
}