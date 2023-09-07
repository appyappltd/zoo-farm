using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AYellowpaper;
using Logic.Interactions;
using Logic.Interactions.Views;
using Logic.Payment;
using NaughtyAttributes;
using Tools.Extension;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class BuildCell : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IConsumer, MonoBehaviour> _consumer;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private ToCameraRotator _toCameraRotator;
        [SerializeField] private List<InteractionView> _interactionViews;
        
        public event Action Build = () => { };

        private void Awake() =>
            _consumer.Value.Bought += OnBought;

        private void OnDestroy() =>
            _consumer.Value.Bought -= OnBought;

        [Conditional("UNITY_EDITOR")]
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position.ChangeY(transform.position.y + 0.5f), Vector3.one);
        }

        public void SetBuildCost(int buildCost) =>
            _consumer.Value.SetCost(buildCost);

        public void SetIcon(Sprite icon) =>
            _icon.sprite = icon;

        public void Reposition(Location toLocation)
        {
            var selfTransform = transform;
            selfTransform.position = toLocation.Position;
            selfTransform.rotation = toLocation.Rotation;
            _toCameraRotator.UpdateRotation();

            for (var index = 0; index < _interactionViews.Count; index++)
            {
                InteractionView view = _interactionViews[index];
                view.SetDefault();
            }
        }

        private void OnBought() =>
            Build.Invoke();

        [Button] [Conditional("UNITY_EDITOR")]
        private void CollectAllInteractionViews()
        {
            _interactionViews = GetComponentsInChildren<InteractionView>().ToList();
        }
    }
}
