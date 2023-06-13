using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Translators
{
    [RequireComponent(typeof(Rigidbody))]
    public class RigidbodyTranslatable : MonoCache, ITranslatable
    {
        private Rigidbody _rigidbody;
        private Vector3 _position;
        
        private void Awake() =>
            _rigidbody ??= GetComponent<Rigidbody>();

        public Vector3 Position => _rigidbody.position;
        public void Warp(Vector3 to)
        {
            _position = to;
        }

        protected override void FixedRun()
        {
            _rigidbody.MovePosition(_position);
        }
    }
}