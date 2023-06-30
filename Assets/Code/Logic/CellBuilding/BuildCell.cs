using System;
using Logic.Wallet;
using Tools.Extension;
using UnityEngine;

namespace Logic.CellBuilding
{
    public class BuildCell : MonoBehaviour
    {
        [SerializeField] private Consumer _consumer;
        [SerializeField] private SpriteRenderer _icon;
        [SerializeField] private ToCameraRotator _toCameraRotator;
        
        public event Action Build = () => { };

        private void Awake() =>
            _consumer.Bought += OnBought;

        private void OnDestroy() =>
            _consumer.Bought -= OnBought;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position.ChangeY(transform.position.y + 0.5f), Vector3.one);
        }

        public void SetBuildCost(int buildCost) =>
            _consumer.SetCost(buildCost);

        public void SetIcon(Sprite icon) =>
            _icon.sprite = icon;

        public void Reposition(Location toLocation)
        {
            transform.position = toLocation.Position;
            transform.rotation = toLocation.Rotation;
            _toCameraRotator.UpdateRotation();
        }

        private void OnBought()
        {
            Build.Invoke();
        }
    }
}
