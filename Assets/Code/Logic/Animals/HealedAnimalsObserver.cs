using System;
using System.Collections.Generic;
using Logic.Medicine;
using Ui.Services;
using Ui.Windows;

namespace Logic.Animals
{
    public class HealedAnimalsReporter
    {
        private readonly List<IMedBedReporter> _bedReporters = new List<IMedBedReporter>();
        private readonly List<AnimalId> _houseWaitingAnimals = new List<AnimalId>();

#if UNITY_EDITOR
        private readonly bool _forceOpenBuildWindow;
#endif
        private readonly IWindowService _windowsService;

#if UNITY_EDITOR
        public HealedAnimalsReporter(IWindowService windowsService, bool forceOpenBuildWindow)
        {
            _forceOpenBuildWindow = forceOpenBuildWindow;
            _windowsService = windowsService;
        }
#endif

#if !UNITY_EDITOR
        public HealedAnimalsReporter(IWindowService windowsService)
        {
            _windowsService = windowsService;
        }
#endif

        ~HealedAnimalsReporter()
        {
            for (int i = 0; i < _bedReporters.Count; i++)
            {
                _bedReporters[i].Healed -= OnHealed;
                _bedReporters[i].HouseFound -= OnHouseFound;
            }
        }

        public void RegisterReporter(IMedBedReporter medBedReporter)
        {
            if (_bedReporters.Contains(medBedReporter))
                return;
            
            _bedReporters.Add(medBedReporter);
            medBedReporter.Healed += OnHealed;
            medBedReporter.HouseFound += OnHouseFound;
        }

        public void GetHealedAnimalType(Action<AnimalId> callback)
        {
            if (_houseWaitingAnimals.Count > 1)
            {
                BuildByWindowChoice(callback);
            }
            else
            {
                
#if UNITY_EDITOR
                if (_forceOpenBuildWindow)
                {
                    BuildByWindowChoice(callback);
                    return;
                }
#endif
                
                BuildForSingle(callback);
            }
        }

        private void OnHouseFound(AnimalId id) =>
            _houseWaitingAnimals.Remove(id);

        private void OnHealed(AnimalId id) =>
            _houseWaitingAnimals.Add(id);

        private void BuildForSingle(Action<AnimalId> callback) =>
            callback.Invoke(_houseWaitingAnimals[0]);

        private void BuildByWindowChoice(Action<AnimalId> callback)
        {
            HouseBuildWindow window = _windowsService.Open(WindowId.BuildHouse).GetComponent<HouseBuildWindow>();
            window.HouseChosen += OnHouseChosen;

            void OnHouseChosen(AnimalId Id)
            {
                callback.Invoke(Id);
                window.HouseChosen -= OnHouseChosen;
            }
        }
    }
}