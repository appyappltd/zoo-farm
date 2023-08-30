using System;
using AYellowpaper;
using Logic.Payment;
using Tools.Extension;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class BuildCell : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IConsumer, MonoBehaviour> _consumer;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private ToCameraRotator _toCameraRotator;
        
        public event Action Build = () => { };

        private void Awake() =>
            _consumer.Value.Bought += OnBought;

        private void OnDestroy() =>
            _consumer.Value.Bought -= OnBought;

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
        }

        private void OnBought() =>
            Build.Invoke();
    }
}
