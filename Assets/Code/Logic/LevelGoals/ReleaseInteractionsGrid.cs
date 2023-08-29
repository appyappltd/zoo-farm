using System;
using System.Collections.Generic;
using Logic.Interactions;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Player;
using Logic.TransformGrid;
using NTC.Global.System;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class ReleaseInteractionsGrid : IDisposable
    {
        private const string ValidationException = "Interaction zone with this type of animal not found";
        
        private readonly Vector3 _undergroundPosition = new Vector3(0f, -10f, 0f);

        private readonly Dictionary<AnimalType, HeroInteraction> _interactions = new Dictionary<AnimalType, HeroInteraction>();
        private readonly List<Action> _disposables = new List<Action>();

        private readonly IGameFactory _gameFactory;
        private readonly ITransformGrid _transformGrid;

        public event Action<AnimalType> ReleaseInteracted = _ => { };

        public ReleaseInteractionsGrid(IGameFactory gameFactory, ITransformGrid transformGrid,
            IEnumerable<AnimalType> releaseTypes)
        {
            _transformGrid = transformGrid;
            _gameFactory = gameFactory;

            foreach (AnimalType type in releaseTypes)
                InitNewInteractionZone(type);
        }

        public void ActivateZone(AnimalType withType)
        {
            if (Validate(withType, out HeroInteraction interaction, false))
                _transformGrid.AddCell(interaction.transform.parent);
        }

        public void DeactivateZone(AnimalType withType)
        {
            if (Validate(withType, out HeroInteraction interaction, true))
                _transformGrid.RemoveCell(interaction.transform.parent);
        }

        public void Dispose()
        {
            for (var index = 0; index < _disposables.Count; index++)
                _disposables[index].Invoke();
        }

        private void InitNewInteractionZone(AnimalType type)
        {
            ReleaseInteractionProvider zone =
                _gameFactory.CreateReleaseInteraction(_undergroundPosition, Quaternion.identity, type);
            zone.gameObject.Disable();
            Subscribe(type, zone.Interaction);
            _interactions.Add(type, zone.Interaction);
        }

        private void Subscribe(AnimalType type, HeroInteraction zone)
        {
            void OnInteracted(Hero _) => ReleaseInteracted.Invoke(type);
            _disposables.Add(() => zone.Interacted -= OnInteracted);
            zone.Interacted += OnInteracted;
        }

        private bool Validate(AnimalType withType, out HeroInteraction interaction, bool shouldBeActive)
        {
            if (_interactions.TryGetValue(withType, out interaction))
                return interaction.gameObject.activeInHierarchy == shouldBeActive;

            throw new NullReferenceException(ValidationException);
        }
    }
}